using System;

namespace PublicLibrary.API.Models
{
    public record UserReadingPaceDto
    {
        public string BookName { get; init; } = string.Empty;
        public DateTime? ReturnDate { get; init; }
        public double ReadingPace { get; init; }
    }
}
