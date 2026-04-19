namespace Talabat.Application.Account.DTOs
{
    public class UserDto
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PictureUrl { get; set; }
        public IList<string> Roles { get; set; } = [];
        public AddressDto Address { get; set; } = new();

        public class AddressDto
        {
            public string? Street { get; set; } = default!;
            public string? City { get; set; } = default!;
            public string? State { get; set; } = default!;
            public string? Country { get; set; } = default!;
            public string? PostalCode { get; set; } = default!;
        }
    }
}
