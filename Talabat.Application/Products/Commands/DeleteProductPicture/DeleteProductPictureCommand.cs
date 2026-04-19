namespace Talabat.Application.Products.Commands.DeleteProductPicture
{
    public record DeleteProductPictureCommand : IRequest
    {
        [JsonIgnore]
        public int ProductId { get; set; }
        public required int PictureId { get; set; }
    }
}