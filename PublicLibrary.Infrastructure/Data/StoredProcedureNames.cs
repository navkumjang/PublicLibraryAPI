namespace PublicLibrary.Infrastructure.Data
{
    internal static class StoredProcedureNames
    {
        public const string GetBooksByBorrowedCount = "dbo.GetBooksByBorrowedCount";
        public const string GetUserLendingRate = "dbo.GetUserLendingRate";
        public const string GetUserReadingPace = "dbo.GetUserReadingPace";
        public const string GetUserBorrowingPatterns = "dbo.GetUserBorrowingPatterns";
    }
}
