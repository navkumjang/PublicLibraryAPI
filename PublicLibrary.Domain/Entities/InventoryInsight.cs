using System;

namespace PublicLibrary.Domain.Entities
{
    public class InventoryInsight
    {
        public int BookId { get; set; }

        public string BookName { get; set; } = string.Empty;

        public int TotalBorrowed { get; set; }
    }
}
