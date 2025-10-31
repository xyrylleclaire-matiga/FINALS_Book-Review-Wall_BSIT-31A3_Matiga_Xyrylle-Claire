using ReviewHall.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReviewHall.Services
{
    public interface ILibraryService
    {
        // Pinalitan ng List<BookListViewModel>
        Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync();

        // Lahat ng methods ay ginawang Async (para maiwasan ang CS0535)
        Task<int> GetTotalBookCountAsync();
        Task<int> GetTotalReviewCountAsync();
        Task<decimal> GetOverallAverageRatingAsync();
        Task<int> GetTotalMembersCountAsync();
    }
}