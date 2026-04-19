
namespace Talabat.Application.Account.Commands.DeleteProfilePicture
{
    internal class DeleteProfilePictureHandler(
        ILogger<DeleteProfilePictureHandler> _logger,
        IUserContext _userContext,
        IFileService _fileService,
        UserManager<User> _userManager,
        IWebHostEnvironment _webHostEnvironment
        ) : IRequestHandler<DeleteProfilePictureCommand>
    {
        public async Task Handle(DeleteProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser() ?? throw new UnAuthorizedException();

            if (string.IsNullOrEmpty(currentUser.Id)) throw new NotFoundException("User ID is required.");

            var dbUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == currentUser.Id, cancellationToken: cancellationToken) 
                ?? throw new NotFoundException("User not found.");

            if (string.IsNullOrEmpty(dbUser.PicturePath))
                throw new NotFoundException("No Image Found!");

            _logger.LogInformation("Deleting profile image for user {UserId}", currentUser.Id);

            _fileService.DeleteFile(_webHostEnvironment.WebRootPath, dbUser.PicturePath);

            dbUser.PicturePath = null;
            var result = await _userManager.UpdateAsync(dbUser);

            if (!result.Succeeded)
            {
                throw new BadRequestException(string.Join(" ", result.Errors.Select(e => e.Description)));
            }
        }
    }
}
