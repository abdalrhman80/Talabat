namespace Talabat.Domain.Shared.Options
{
    public sealed class EmailSettingsOptions
    {
        public string SenderEmail { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string Host { get; set; } = default!;
        public int Port { get; set; }
    }
}
