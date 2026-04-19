using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Order.Commands.AddRefundRequest;
using Talabat.Application.Order.Commands.CancelOrderPayment;
using Talabat.Application.Order.Commands.ConfirmOrderPayment;
using Talabat.Application.Order.Commands.CreateOrder;
using Talabat.Application.Order.Commands.FailedOrderPayment;
using Talabat.Application.Order.Commands.RefundOrderPayment;
using Talabat.Application.Order.Commands.RejectRefundRequest;
using Talabat.Application.Order.Commands.UpdateOrderStatus;
using Talabat.Application.Order.Queries.GeInvoice;
using Talabat.Application.Order.Queries.GetDeliveryMethods;
using Talabat.Application.Order.Queries.GetOrdersToAdmin;
using Talabat.Application.Order.Queries.GetRefundRequest;
using Talabat.Application.Order.Queries.GetRefundRequestsToAdmin;
using Talabat.Application.Order.Queries.GetRefundRequestToAdmin;
using Talabat.Application.Order.Queries.GetUserOrder;
using Talabat.Application.Order.Queries.GetUserOrders;
using Talabat.Application.Order.Queries.PaymentMethods;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class OrderController(IMediator _mediator) : ControllerBase
    {
        [HttpPost("orders")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);

            Response.Cookies.Append($"order{result.Order.Id}_payment_data", result.PaymentData.ToString()!, new CookieOptions { HttpOnly = false, Secure = true });

            return Ok(new { Message = "Your order has been created successfully", Invoice = (object)result.Invoice!, result.Order });
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetUserOrders([FromQuery] GetUserOrdersQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("admin/orders")]
        public async Task<IActionResult> GetOrdersToAdmin([FromQuery] GetOrdersToAdminQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            var result = await _mediator.Send(new GetUserOrderQuery(id));
            return Ok(result);
        }

        [HttpPut("admin/orders/{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus([FromRoute] int id, [FromQuery] UpdateOrderStatusCommand command)
        {
            command.OrderId = id;
            await _mediator.Send(command);
            return Ok();
        }


        [HttpGet("orders/paymentMethods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var result = await _mediator.Send(new GetPaymentMethodsQuery());
            return Ok(result);
        }


        [HttpGet("orders/deliveryMethods")]
        public async Task<IActionResult> GetDeliveryMethods()
        {
            var result = await _mediator.Send(new GetDeliveryMethodsQuery());
            return Ok(result);
        }


        [HttpGet("orders/invoice")]
        public async Task<IActionResult> GetInvoice([FromQuery(Name = "invoice_id")] int invoiceId)
        {
            var result = await _mediator.Send(new GeInvoiceQuery(invoiceId));
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("orders/success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery(Name = "invoice_id")] string invoiceId)
        {
            return Ok(new { Message = "Payment successful", InvoiceId = invoiceId });
        }


        [AllowAnonymous]
        [HttpGet("orders/failure")]
        public async Task<IActionResult> PaymentFailure([FromQuery(Name = "invoice_id")] string invoiceId, [FromQuery] string? errorMessage)
        {
            return Ok(new { Message = "Payment failed", InvoiceId = invoiceId });
        }


        [AllowAnonymous]
        [HttpGet("orders/pending")]
        public async Task<IActionResult> PaymentPending([FromQuery(Name = "invoice_id")] string invoiceId)
        {
            return Ok(new { Message = "Payment pending", InvoiceId = invoiceId });
        }


        [AllowAnonymous]
        [HttpPost("orders/webhook/paid_json")]
        public async Task<IActionResult> WebhookPaidJson([FromBody] ConfirmOrderPaymentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("orders/webhook/failed")]
        public async Task<IActionResult> WebhookFailed([FromForm] FailedOrderPaymentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("orders/webhook/cancel")]
        public async Task<IActionResult> WebhookCancel([FromForm] CancelOrderPaymentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        [AllowAnonymous]
        [HttpPost("orders/webhook/refund")]
        public async Task<IActionResult> WebhookRefund([FromBody] RefundOrderPaymentCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }


        [HttpPost("orders/{id}/refund-request")]
        public async Task<IActionResult> AddRefundRequest([FromRoute] int id, [FromBody] AddRefundRequestQuery command)
        {
            command.OrderId = id;
            await _mediator.Send(command);
            return CreatedAtAction(nameof(GeRefundRequest), new { id }, null);
        }


        [HttpGet("orders/{id}/refund-request")]
        public async Task<IActionResult> GeRefundRequest([FromRoute] int id)
        {
            var refundRequest = await _mediator.Send(new GetRefundRequestQuery(id));
            return Ok(refundRequest);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("admin/orders/refund-requests")]
        public async Task<IActionResult> GetRefundRequests([FromQuery] GetRefundRequestsToAdminQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpGet("admin/orders/refund-requests/{id}")]
        public async Task<IActionResult> GetRefundRequestToAdmin(int id)
        {
            var result = await _mediator.Send(new GetRefundRequestToAdminQuery(id));
            return Ok(result);
        }

        [Authorize(Roles = RoleNames.Admin)]
        [HttpPut("admin/orders/refund-requests/{id}/reject")]
        public async Task<IActionResult> RejectRefundRequest([FromRoute] int id, [FromBody] RejectRefundRequestCommand command)
        {
            command.RequestId = id;
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
