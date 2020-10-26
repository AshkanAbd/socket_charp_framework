using Microsoft.EntityFrameworkCore;

namespace socket_sharp.Models
{
    public class DbContext : SoftDeletes.Core.DbContext
    {
        protected DbContext()
        {
        }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}