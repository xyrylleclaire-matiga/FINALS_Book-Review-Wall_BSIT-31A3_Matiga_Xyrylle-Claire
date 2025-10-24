namespace Library_Management.Utility
{
    // Ginagamit ang static class para hindi na kailangan pang i-instantiate
    public static class WC
    {
        // Ang mga constants na ito ang gagamitin sa User.IsInRole() at sa Identity seeding
        public const string AdminRole = "Admin";
        public const string ReviewerRole = "Reviewer"; // Pinalitan ko ang "UserRole" para mas descriptive
    }
}