namespace PublicLibrary.API.Models
{
    public record UserLendingRateDto
    {
        public string Name { get; init; } = string.Empty;
        public int BorrowCount { get; init; }
    }
}
