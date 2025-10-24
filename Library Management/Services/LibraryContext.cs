using Library_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Library_Management.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options)
            : base(options)
        {
        }

        public DbSet<BookListViewModel> Books { get; set; }
        public DbSet<Member> Members { get; set; }

    }
}
