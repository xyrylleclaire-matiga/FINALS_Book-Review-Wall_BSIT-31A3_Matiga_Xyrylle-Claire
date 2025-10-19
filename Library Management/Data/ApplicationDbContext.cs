using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Library_Management.Models;

namespace Library_Management.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<AddBookViewModel> AddBookViewModels { get; set; }
        public DbSet<BookListViewModel> BookListViewModels { get; set; }
        public DbSet <EditBookViewModel> EditBookViewModels { get; set; }
        //public DbSet <ErrorViewModel> ErrorViewModels { get; set; }
    }

}
