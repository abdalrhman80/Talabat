using Talabat.Domain.Shared;

namespace Talabat.Domain.Services
{
    public interface IUserContext
    {
        CurrentUser? GetCurrentUser();
    }
}
