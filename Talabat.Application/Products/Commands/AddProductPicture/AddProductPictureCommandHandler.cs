namespace Talabat.Application.Products.Commands.AddProductPicture
{
    internal class AddProductPictureCommandHandler(
        ILogger<AddProductPictureCommandHandler> _logger,
        IUserContext _userContext,
        IFileService _fileService,
        IWebHostEnvironment _webHostEnvironment,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
        ) : IRequestHandler<AddProductPictureCommand, ProductDto>
    {
        public async Task<ProductDto> Handle(AddProductPictureCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            if (request.Picture is null)
                throw new BadRequestException("No picture file provided.");

            if (!FileSettings.AllowedExtensions.Contains(Path.GetExtension(request.Picture.FileName).ToLower()))
                throw new BadRequestException($"Allowed extensions are: {string.Join(", ", FileSettings.AllowedExtensions)}");

            if (request.Picture.Length > FileSettings.MaxImageSize)
                throw new BadRequestException($"File size must not exceed {FileSettings.MaxImageSize / 1024 / 1024} MB.");

            var existingProduct = await _unitOfWork.Repository<Product>().GetEntityWithSpecificationAsync(new ProductSpecifications(request.ProductId))
                ?? throw new NotFoundException($"Product with id {request.ProductId} not found.");

            if (existingProduct != null && existingProduct.ProductPictures.Count >= 5)
                throw new BadRequestException("The maximum number for a product pictures is 5");

            _logger.LogInformation("User {UserId} is uploading a new picture for product {ProductId}", currentUser.Id, request.ProductId);

            var filePath = await _fileService.UploadFileAsync(request.Picture, _webHostEnvironment.WebRootPath, FileSettings.ProductPicturesFolderPath);

            var productPicture = new ProductPicture()
            {
                PicturePath = filePath,
                ProductId = request.ProductId,
            };

            existingProduct!.ProductPictures.Add(productPicture);

            _unitOfWork.Repository<Product>().Update(existingProduct);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<ProductDto>(existingProduct);
        }
    }
}
