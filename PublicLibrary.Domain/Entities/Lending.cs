using System;

namespace PublicLibrary.Domain.Entities
{
    public class Lending
    {
        public Guid Id { get; set; }

        public Guid BorrowerId { get; set; }

        public Guid BookId { get; set; }

        public DateOnly FromDate { get; set; }

        public DateOnly ToDate { get; set; }

        public DateTime LendingDate { get; set; }
    }
}
