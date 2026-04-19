using System.Linq.Expressions;
using Talabat.Domain.Models;
using Talabat.Domain.Shared;
using Talabat.Domain.Shared.Params;
using Talabat.Domain.Specifications;

namespace Talabat.Domain.Specifications.EntitiesSpecifications
{
    public class RefundRequestSpecifications : SpecificationBase<RefundRequest>
    {
        public RefundRequestSpecifications(RefundRequestParams requestParams)
        {
            AddIncludes(r => r.Order);
            AddIncludes($"{nameof(RefundRequest.Order)}.{nameof(Order.DeliveryMethod)}");

            if (requestParams.IncludeOrderItems) AddIncludes($"{nameof(RefundRequest.Order)}.{nameof(Order.OrderItems)}");

            if (requestParams.RequestStatus.HasValue) AddCriteria(r => r.Status == requestParams.RequestStatus.Value);

            ApplyOrderByDescending(r => r.RequestedAt);

            ApplyPagination(requestParams.PageSize, (requestParams.PageNumber - 1) * requestParams.PageSize);
        }

        public RefundRequestSpecifications(string? buyerEmail = null, int? orderId = null)
        {
            var expressions = new List<Expression<Func<RefundRequest, bool>>>();

            if (!string.IsNullOrEmpty(buyerEmail)) expressions.Add(r => r.BuyerEmail == buyerEmail);

            if (orderId.HasValue) expressions.Add(r => r.OrderId == orderId.Value);

            if (expressions.Count > 0) AddCriteria(expressions.Aggregate((current, next) => current.And(next)));


            ApplyOrderByDescending(r => r.RequestedAt);
        }

        public RefundRequestSpecifications(int requestId, bool includeOrder = false, bool includeOrderItems = false)
        {
            if (includeOrder)
            {
                AddIncludes(r => r.Order);
                AddIncludes($"{nameof(RefundRequest.Order)}.{nameof(Order.DeliveryMethod)}");
            }

            if (includeOrderItems) AddIncludes($"{nameof(RefundRequest.Order)}.{nameof(Order.OrderItems)}");

            AddCriteria(r => r.Id == requestId);
        }
    }
}
