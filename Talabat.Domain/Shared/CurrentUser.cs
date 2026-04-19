namespace Talabat.Domain.Shared
{
    public record CurrentUser(string Id, string UserName, string FirstName, string LastName, string Email, string Phone, IEnumerable<string> Roles);
}
