namespace PublicLibrary.API.Models
{
    public record InventoryInsightDto
    {
        public string BookName { get; init; } = string.Empty;
        public int TotalBorrowed { get; init; }
    }
}
