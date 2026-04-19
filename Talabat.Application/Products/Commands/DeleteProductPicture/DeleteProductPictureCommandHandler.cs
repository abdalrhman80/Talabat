namespace Talabat.Application.Products.Commands.DeleteProductPicture
{
    internal class DeleteProductPictureCommandHandler(
        ILogger<DeleteProductPictureCommandHandler> _logger,
        IUserContext _userContext,
        IFileService _fileService,
        IWebHostEnvironment _webHostEnvironment,
        IUnitOfWork _unitOfWork
        ) : IRequestHandler<DeleteProductPictureCommand>
    {
        public async Task Handle(DeleteProductPictureCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            var existingProductPicture = await _unitOfWork.Repository<ProductPicture>().GetEntityWithSpecificationAsync(new ProductPictureSpecifications(request.ProductId, request.PictureId))
                ?? throw new NotFoundException($"No picture found!.");

            _logger.LogInformation("User {UserId} Deleting product {ProductId} picture {PictureId}", currentUser.Id, request.ProductId, request.PictureId);

            _fileService.DeleteFile(_webHostEnvironment.WebRootPath, existingProductPicture.PicturePath!);

            _unitOfWork.Repository<ProductPicture>().Delete(existingProductPicture);
            await _unitOfWork.CompleteAsync();
        }
    }
}
