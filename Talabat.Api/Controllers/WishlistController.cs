using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Wishlist.Commands.AddToWishlist;
using Talabat.Application.Wishlist.Commands.RemoveFromWishlist;
using Talabat.Application.Wishlist.Queries.GetWishlist;

namespace Talabat.Api.Controllers
{
    [Route("api/wishlist")]
    [ApiController]
    [Authorize]
    public class WishlistController(IMediator _mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetWishlist([FromQuery] GetWishlistQuery query)
        {
            var wishList = await _mediator.Send(query);
            return Ok(wishList);
        }

        [HttpPost]
        public async Task<IActionResult> AddToWishlist([FromQuery] int productId)
        {
            await _mediator.Send(new AddToWishlistCommand(productId));
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveFromWishlist([FromQuery] int productId)
        {
            await _mediator.Send(new RemoveFromWishlistCommand(productId));
            return NoContent();
        }
    }
}
