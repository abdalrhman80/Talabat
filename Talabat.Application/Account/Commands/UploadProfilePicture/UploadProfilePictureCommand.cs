using Microsoft.AspNetCore.Http;

namespace Talabat.Application.Account.Commands.UploadProfilePicture
{
    public record UploadProfilePictureCommand(IFormFile Picture) : IRequest;
}
