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
            var connectionString = "Data Source=LibraryDb.db";

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string is hardcoded but empty.");
            }

            builder.UseSqlite(connectionString);

            return new ApplicationDbContext(builder.Options);
        }
    }
}