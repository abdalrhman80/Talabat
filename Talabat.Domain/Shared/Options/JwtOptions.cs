namespace Talabat.Domain.Shared.Options
{
    public sealed class JwtOptions
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public double ExpirationInMinutes { get; set; }
        public int ExpirationInDays { get; set; }
    }
}
