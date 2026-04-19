namespace Talabat.Application.Order.Queries.GetDeliveryMethods
{
    internal class GetDeliveryMethodsQueryHandler(
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<GetDeliveryMethodsQuery, IReadOnlyList<DeliveryMethod>>
    {
        public async Task<IReadOnlyList<DeliveryMethod>> Handle(GetDeliveryMethodsQuery request, CancellationToken cancellationToken)
        {
            var deliveryMethods = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync(); ;
            return deliveryMethods;
        }
    }
}
