using System;

namespace PublicLibrary.Domain.Entities
{
    public class UserReadingPace
    {
        public int BorrowerId { get; set; }
        public string BookName { get; set; } = string.Empty;
        public System.DateTime ReturnDate { get; set; }
        public double ReadingPace { get; set; }
    }
}
