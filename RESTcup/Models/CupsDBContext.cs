using Microsoft.EntityFrameworkCore;

namespace RESTcup.Models
{
    public class CupsDBContext: DbContext
    // NuGet package Microsoft.EntityFrameworkCore.SqlServer
    // same major version as your .NET version
    {
        public CupsDBContext(DbContextOptions<CupsDBContext> options) : base(options)
        {
        }

        public DbSet<Cup> Cups { get; set; }
    }
}
