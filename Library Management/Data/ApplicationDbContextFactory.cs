using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration; 
using System.IO;

namespace Library_Management.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {

        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=LibraryDb;Trusted_Connection=True;MultipleActiveResultSets=true";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is hardcoded but empty.");
            }

            builder.UseSqlServer(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}