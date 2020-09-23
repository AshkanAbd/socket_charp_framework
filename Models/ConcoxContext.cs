using Microsoft.EntityFrameworkCore;
using DbContext = SoftDeletes.Core.DbContext;

namespace socket_sharp.Models
{
    public class ConcoxContext : DbContext
    {
        protected ConcoxContext()
        {
        }

        public ConcoxContext(DbContextOptions<ConcoxContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}