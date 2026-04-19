using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;
using Talabat.Domain.Shared.Constants;
using Talabat.Domain.Shared.Params;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class OrderSpecifications : SpecificationBase<Order>
    {
        public OrderSpecifications(OrderParams orderParams)
        {
            AddIncludes(o => o.OrderItems);

            AddIncludes(o => o.DeliveryMethod);

            ApplyFiltering(orderParams);

            ApplySorting(orderParams.SortBy, orderParams.SortDirection);

            ApplyPagination(orderParams.PageSize, (orderParams.PageNumber - 1) * orderParams.PageSize);
        }

        public OrderSpecifications(string buyerEmail, OrderParams orderParams)
        {
            AddIncludes(o => o.OrderItems);

            AddIncludes(o => o.DeliveryMethod);

            ApplyFiltering(orderParams, buyerEmail);

            ApplySorting(orderParams.SortBy, orderParams.SortDirection);

            ApplyPagination(orderParams.PageSize, (orderParams.PageNumber - 1) * orderParams.PageSize);
        }

        public OrderSpecifications(string? buyerEmail = null, int? orderId = null, int? productId = null, bool addOrderItems = false, bool addDeliveryMethod = false)
        {
            if (addOrderItems) AddIncludes(o => o.OrderItems);

            if (addDeliveryMethod) AddIncludes(o => o.DeliveryMethod);

            ApplyFiltering(buyerEmail, orderId, productId);
        }

        public OrderSpecifications(string buyerEmail, string invoiceId)
        {
            ApplyFiltering(buyerEmail, invoiceId);
        }

        public OrderSpecifications(string invoiceId, bool addOrderItems = false)
        {
            if (addOrderItems) AddIncludes(o => o.OrderItems);
            AddCriteria(o => o.InvoiceId == invoiceId);
        }


        #region Helper Methods

        private void ApplyFiltering(OrderParams orderParams, string? buyerEmail = null)
        {
            var expressions = new List<Expression<Func<Order, bool>>>();

            if (buyerEmail is not null) expressions.Add(o => o.BuyerEmail == buyerEmail);

            if (orderParams.PaymentMethod.HasValue) expressions.Add(o => o.PaymentMethod.NameEn.Contains(orderParams.PaymentMethod.Value.ToString()));

            if (orderParams.Status.HasValue) expressions.Add(o => o.Status == orderParams.Status);

            if (expressions.Count > 0) AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }

        private void ApplyFiltering(string? buyerEmail = null, int? orderId = null, int? productId = null)
        {
            var expressions = new List<Expression<Func<Order, bool>>>();

            if (!string.IsNullOrEmpty(buyerEmail)) expressions.Add(o => o.BuyerEmail == buyerEmail);

            if (orderId.HasValue) expressions.Add(o => o.Id == orderId.Value);

            if (productId.HasValue) expressions.Add(o => o.OrderItems.Any(oi => oi.Product.Id == productId.Value));

            if (expressions.Count > 0) AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }

        private void ApplyFiltering(string buyerEmail, string invoiceId)
        {
            var expressions = new List<Expression<Func<Order, bool>>>
            {
                o => o.BuyerEmail == buyerEmail,
                o => o.InvoiceId == invoiceId
            };

            if (expressions.Count > 0) AddCriteria(expressions.Aggregate((current, next) => current.And(next)));
        }

        private void ApplySorting(string? sortBy, SortDirection sortDirection)
        {
            var key = sortBy?.Trim().ToLower() ?? string.Empty;

            var sortSelectors = new Dictionary<string, Expression<Func<Order, object>>>
            {
                { nameof(Order.OrderDate).ToLower(), o => o.OrderDate },
                { nameof(Order.SubTotal).ToLower(), o => o.SubTotal },
            };

            if (string.IsNullOrEmpty(key) || !sortSelectors.ContainsKey(key))
            {
                key = nameof(Order.OrderDate).ToLower();
            }

            var selector = sortSelectors[key];

            if (sortDirection == SortDirection.Ascending)
            {
                ApplyOrderBy(selector);
            }
            else
            {
                ApplyOrderByDescending(selector);
            }
        }

        #endregion
    }
}
