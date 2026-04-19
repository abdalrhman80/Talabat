using Microsoft.AspNetCore.Http;

namespace Talabat.Application.Products.Commands.AddProductPicture
{
    public class AddProductPictureCommand : IRequest<ProductDto>
    {
        [JsonIgnore]
        public int ProductId { get; set; }

        public required IFormFile Picture { get; set; }
    }
}
