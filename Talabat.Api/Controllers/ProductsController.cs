using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.Application.Products.Commands.AddProductPicture;
using Talabat.Application.Products.Commands.CreateProduct;
using Talabat.Application.Products.Commands.DeleteProduct;
using Talabat.Application.Products.Commands.DeleteProductPicture;
using Talabat.Application.Products.Commands.UpdateProduct;
using Talabat.Application.Products.Queries.GetAllProducts;
using Talabat.Application.Products.Queries.GetProduct;
using Talabat.Application.Products.Queries.GetProductBrands;
using Talabat.Application.Products.Queries.GetProductTypes;
using Talabat.Application.Reviews.Commands.AddReview;
using Talabat.Application.Reviews.Commands.AdminDeleteReview;
using Talabat.Application.Reviews.Commands.DeleteReview;
using Talabat.Application.Reviews.Commands.UpdateReview;
using Talabat.Application.Reviews.Queries.GetProductReview;
using Talabat.Application.Reviews.Queries.GetProductReviews;
using Talabat.Domain.Shared.Constants;

namespace Talabat.Api.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class ProductController(IMediator _mediator) : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var result = await _mediator.Send(new GetProductQuery(id));
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("products/types")]
        public async Task<IActionResult> GetProductTypes([FromQuery] GetProductTypesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("products/brands")]
        public async Task<IActionResult> GetProductBrands([FromQuery] GetProductBrandsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }


        [Authorize(Roles = $"{RoleNames.Admin}")]
        [HttpPost("admin/products")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand request)
        {
            var id = await _mediator.Send(request);
            return CreatedAtAction(nameof(GetProduct), new { id }, null);
        }


        [Authorize(Roles = $"{RoleNames.Admin}")]
        [HttpPut("admin/products/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand request)
        {
            request.Id = id;
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("admin/products/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpPost("admin/products/{id}/picture")]
        public async Task<IActionResult> AddProductPicture(int id, [FromForm] AddProductPictureCommand request)
        {
            request.ProductId = id;
            var result = await _mediator.Send(request);
            return Ok(result);
        }


        [Authorize(Roles = RoleNames.Admin)]
        [HttpDelete("admin/products/{id}/picture")]
        public async Task<IActionResult> DeleteProductPicture(int id, [FromQuery] DeleteProductPictureCommand request)
        {
            request.ProductId = id;
            await _mediator.Send(request);
            return NoContent();
        }


        [AllowAnonymous]
        [HttpGet("products/{productId}/reviews")]
        public async Task<IActionResult> GetProductReviews(int productId)
        {
            var reviews = await _mediator.Send(new GetProductReviewsQuery(productId));
            var averageRating = reviews.Count > 0 ? Math.Round(reviews.Average(r => r.Rating), 1) : 0;
            var totalReviews = reviews.Count;
            return Ok(new { AverageRating = averageRating, TotalReviews = totalReviews, Reviews = reviews });
        }


        [AllowAnonymous]
        [HttpGet("products/{productId}/reviews/{reviewId}")]
        public async Task<IActionResult> GetProductReview(int productId, int reviewId)
        {
            var review = await _mediator.Send(new GetProductReviewQuery(productId, reviewId));
            return Ok(review);
        }


        [Authorize]
        [HttpPost("products/{productId}/reviews")]
        public async Task<IActionResult> AddReview(int productId, [FromBody] AddReviewCommand command)
        {
            command.ProductId = productId;
            var review = await _mediator.Send(command);
            return Ok(review);
        }


        [Authorize]
        [HttpPut("products/reviews/{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] UpdateReviewCommand command)
        {
            command.ReviewId = reviewId;
            var review = await _mediator.Send(command);
            return Ok(review);
        }


        [HttpDelete("products/reviews/{reviewId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            await _mediator.Send(new DeleteReviewCommand(reviewId));
            return NoContent();
        }


        [HttpDelete("admin/products/reviews/{reviewId}")]
        [Authorize(Roles = RoleNames.Admin)]
        public async Task<IActionResult> DeleteReviewAsAdmin(int reviewId)
        {
            await _mediator.Send(new AdminDeleteReviewCommand(reviewId));
            return NoContent();
        }
    }
}
