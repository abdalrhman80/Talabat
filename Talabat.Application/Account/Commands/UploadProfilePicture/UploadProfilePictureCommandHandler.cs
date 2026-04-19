namespace Talabat.Application.Account.Commands.UploadProfilePicture
{
    internal class UploadProfilePictureCommandHandler(
        ILogger<UploadProfilePictureCommandHandler> _logger,
        IUserContext _userContext,
        IFileService _fileService,
        UserManager<User> _userManager,
        IWebHostEnvironment _webHostEnvironment
        ) : IRequestHandler<UploadProfilePictureCommand>
    {
        public async Task Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            if (request.Picture is null)
                throw new BadRequestException("No picture file provided.");

            if (!FileSettings.AllowedExtensions.Contains(Path.GetExtension(request.Picture.FileName).ToLower()))
                throw new BadRequestException($"Allowed extensions are: {string.Join(", ", FileSettings.AllowedExtensions)}");

            if (request.Picture.Length > FileSettings.MaxImageSize)
                throw new BadRequestException($"File size must not exceed {FileSettings.MaxImageSize / 1024 / 1024} MB.");

            var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUser.Id) ?? throw new NotFoundException("User not found.");

            _logger.LogInformation("User {UserId} is uploading a new profile image", currentUser.Id);

            if (!string.IsNullOrEmpty(dbUser.PicturePath))
            {
                _fileService.DeleteFile(_webHostEnvironment.WebRootPath, dbUser.PicturePath);
            }

            var filePath = await _fileService.UploadFileAsync(request.Picture, _webHostEnvironment.WebRootPath, FileSettings.UserPicturesFolderPath);

            dbUser.PicturePath = filePath;

            var result = await _userManager.UpdateAsync(dbUser);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
