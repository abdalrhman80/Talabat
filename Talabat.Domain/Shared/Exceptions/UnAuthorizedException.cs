namespace Talabat.Domain.Shared.Exceptions
{
    public class UnAuthorizedException(string? message = null) : Exception(message ?? "You Are Not Authorized")
    {
    }
}
