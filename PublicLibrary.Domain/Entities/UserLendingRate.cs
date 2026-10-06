using System;

namespace PublicLibrary.Domain.Entities
{
    public class UserLendingRate
    {
        public int BorrowerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int BorrowCount { get; set; }
    }
}
