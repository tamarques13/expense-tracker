namespace Spentir.Application.DTOs
{
    public sealed class ReceiptDto
    {
        public string Category { get; set; } = string.Empty;
        public decimal Total { get; set; }
    }
}