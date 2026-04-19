namespace Talabat.Application.Auth.DTOs
{
    public record UserDto(
        string Id,
        string Email,
        string Username,
        string FirstName,
        string LastName,
        List<string> Roles);
}
