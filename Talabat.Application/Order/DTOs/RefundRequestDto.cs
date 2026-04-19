namespace Talabat.Application.Order.DTOs
{
    public class RefundRequestDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public required string Reason { get; set; }
        public required string Status { get; set; }
        public required string RequestedAt { get; set; }
        public string? ReviewedAt { get; set; }

        [JsonPropertyName("notes")]
        public string? AdminNotes { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OrderDto? Order { get; set; }
    }
}
