using System;

namespace PublicLibrary.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public int TotalPages { get; set; }

        public decimal Price { get; set; }

        public int TotalCount { get; set; }

        public int TotalBorrowed { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
