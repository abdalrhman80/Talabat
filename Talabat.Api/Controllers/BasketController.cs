using MediatR;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Basket.Command.CreateCustomerBasket;
using Talabat.Application.Basket.Command.DeleteCustomerBasket;
using Talabat.Application.Basket.Queries.GetCustomerBasket;

namespace Talabat.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCustomerBasket(string id)
        {
            var result = await _mediator.Send(new GetCustomerBasketQuery(id));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket(CreateOrUpdateCustomerBasketCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            await _mediator.Send(new DeleteCustomerBasketCommand(id));
            return NoContent();
        }
    }
}
