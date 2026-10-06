using System;

namespace PublicLibrary.Domain.Entities
{
    public class UserBorrowingPattern
    {
        public int BookId { get; set; }
        public string BookName { get; set; } = string.Empty;
    }
}
