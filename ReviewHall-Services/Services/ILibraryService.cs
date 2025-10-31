using ReviewHall.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReviewHall.Services
{
    public interface ILibraryService
    {
        Task<List<BookListViewModel>> GetAllBooksWithReviewsAsync();

        Task<int> GetTotalBookCountAsync();
        Task<int> GetTotalReviewCountAsync();
        Task<decimal> GetOverallAverageRatingAsync();
        Task<int> GetTotalMembersCountAsync();
    }
}