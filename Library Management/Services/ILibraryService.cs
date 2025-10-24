using Library_Management.Models;

namespace Library_Management.Services
{
    public interface ILibraryService
    {
        Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync();
        int GetTotalReviewCount();
        decimal GetOverallAverageRating();
        int GetTotalMembersCount();
    }
}
