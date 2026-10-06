using System;

namespace PublicLibrary.Domain.Entities
{
    public class Borrower
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}
