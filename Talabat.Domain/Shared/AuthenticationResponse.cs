using System.Text.Json.Serialization;

namespace Talabat.Domain.Shared
{
    public class AuthenticationResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Message { get; set; }
        public string UserName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public List<string> Roles { get; set; } = [];
        public string Token { get; set; } = default!;
        public DateTime TokenExpiration { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
