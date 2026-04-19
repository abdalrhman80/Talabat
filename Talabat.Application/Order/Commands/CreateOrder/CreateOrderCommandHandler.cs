using static Talabat.Domain.Models.OrderItem;
using static Talabat.Domain.Shared.Payment.FawaterakInvoiceRequestModel;

namespace Talabat.Application.Order.Commands.CreateOrder
{
    internal class CreateOrderCommandHandler(
        ILogger<CreateOrderCommandHandler> _logger, 
        IUnitOfWork _unitOfWork,
        IUserContext _userContext,
        IFawaterakPaymentService _fawaterakPaymentService,
        IMapper _mapper,
        IBasketService _basketService,
        IConfiguration _configuration
        ) : IRequestHandler<CreateOrderCommand, OrderWithInvoiceDto>
    {
        public async Task<OrderWithInvoiceDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            #region Pseudo Code
            // 1. Validate the BasketId && Payment Method Id
            // 2. Retrieve the Basket items from the database or cache
            // 3. validate the items in the Basket
            // 4. Calculate the total price of the items in the Basket
            // 5. create an order record in the database with status "Pending"
            // 6. Create the invoice using FawaterakPaymentService
            // 7. Save the invoice id in the order record & order id in the invoice payload
            // 8. Return the order details along with the invoice link to the client
            #endregion

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

                var order = await CreateOrderAsync(
                            request.BasketId,
                            request.DeliveryMethodId,
                            request.PaymentMethodId,
                            _mapper.Map<ShippingAddress>(request.ShippingAddress)
                            );

                var invoice = new FawaterakInvoiceRequestModel()
                {
                    PaymentMethodId = request.PaymentMethodId,
                    Customer = new CustomerModel()
                    {
                        CustomerId = currentUser.Id,
                        FirstName = currentUser.FirstName,
                        LastName = currentUser.LastName,
                        Email = currentUser.Email,
                        Phone = currentUser.Phone,
                    },
                    CartItems = [.. order.OrderItems.Select(oi => new CartItemModel
                    {
                        Name = oi.Product.Name,
                        Price = oi.Price,
                        Quantity = oi.Quantity
                    })],
                    Shipping = order.GetShipping(),
                    PayLoad = new InvoicePayload()
                    {
                        OrderId = order.Id.ToString()
                    },
                    RedirectionUrls = new RedirectionUrlsModel()
                    {
                        OnSuccess = $"{_configuration["ApiBaseUrl"]}/api/orders/success",
                        OnFailure = $"{_configuration["ApiBaseUrl"]}/api/orders/failure",
                        OnPending = $"{_configuration["ApiBaseUrl"]}/api/orders/pending"
                    }
                };

                (var fawaterakResponse, var paymentData) = await _fawaterakPaymentService.Pay(invoice) ?? throw new InvalidOperationException("Payment failed");

                order.InvoiceId = fawaterakResponse?.InvoiceId.ToString() ?? "";

                _unitOfWork.Repository<UserOrder>().Update(order);

                await _unitOfWork.CompleteAsync();
                await _unitOfWork.CommitTransactionAsync();

                _logger.LogInformation("User {UserId} create new order {OrderId}", currentUser.Id, order.Id);

                var result = new OrderWithInvoiceDto
                {
                    Order = _mapper.Map<OrderDto>(order),
                    Invoice = fawaterakResponse!,
                    PaymentData = paymentData!
                };

                return result;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task<UserOrder> CreateOrderAsync(string basketId, int deliveryMethodId, int paymentMethodId, ShippingAddress shippingAddress)
        {
            // 1.Get Basket
            var basket = await _basketService.GetCustomerBasketAsync(basketId);

            // 2.Get Selected Items at Basket From Product Table 
            var orderItems = new List<OrderItem>();

            // 2.1.We will update the products stock quantity after creating the order, so we need to keep track of them in order to update them later
            var updatedProducts = new List<Product>();

            if (basket?.BasketItems.Count > 0)
            {
                foreach (var item in basket.BasketItems)
                {
                    var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationAsync(new ProductSpecifications(item.Id))
                        ?? throw new NotFoundException($"No product found with this id {item.Id}");

                    if (product.StockQuantity < item.Quantity)
                        throw new BadRequestException($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}");

                    var productItemOrdered = new ProductItemOrdered()
                    {
                        Id = product.Id,
                        Name = product.Name!,
                        Description = product.Description!,
                        PicturePath = item.Picture != null
                        ? product.ProductPictures.FirstOrDefault(p => p.Id == item.Picture.Id)?.PicturePath ?? string.Empty
                        : product.ProductPictures.FirstOrDefault()?.PicturePath ?? string.Empty,
                    };

                    var orderItem = new OrderItem()
                    {
                        Product = productItemOrdered,
                        Price = product.Price,
                        Quantity = item.Quantity
                    };

                    orderItems.Add(orderItem);

                    updatedProducts.Add(product);
                }
            }
            else
            {
                throw new NotFoundException("No basket found!");
            }

            // 3.Calculate SubTotal
            var subTotal = orderItems.Sum(orderItem => orderItem.Price * orderItem.Quantity);

            // 4.Get Payment Method
            var paymentMethods = await _fawaterakPaymentService.GetPaymentMethods() ?? throw new NotFoundException($"No payment methods found from Fawaterak!");
            var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.PaymentId == paymentMethodId)!;

            // 5.Get Delivery Method 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId)
                ?? throw new NotFoundException($"No delivery method found with this id {deliveryMethodId}");

            // 6.Create Order
            var order = new UserOrder()
            {
                BuyerEmail = _userContext.GetCurrentUser()?.Email ?? throw new NotFoundException("No user found"),
                Status = OrderStatus.Pending,
                ShippingAddress = shippingAddress,
                DeliveryMethodId = deliveryMethodId,
                PaymentMethod = new UserOrder.FawaterakPaymentMethod()
                {
                    Id = paymentMethod.PaymentId,
                    NameEn = paymentMethod.NameEn,
                    NameAr = paymentMethod.NameAr,
                    Logo = paymentMethod.Logo,
                    Redirect = bool.Parse(paymentMethod.Redirect)
                },
                OrderItems = orderItems,
                SubTotal = subTotal,
            };

            // 7.Add Order Locally
            _unitOfWork.Repository<UserOrder>().Add(order);

            // 8.Update products with new quantity
            updatedProducts.ForEach(product =>
            {
                product.StockQuantity -= order.OrderItems.First(oi => oi.Product.Id == product.Id).Quantity;
            });

            _unitOfWork.Repository<Product>().UpdateRange(updatedProducts);

            // 9.SaveChanges 
            await _unitOfWork.CompleteAsync();

            // 10.Remove Basket
            await _basketService.DeleteCustomerBasketAsync(basketId);

            return order;
        }
    }
}
