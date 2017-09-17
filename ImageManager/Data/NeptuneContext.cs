using System;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Data.Domains;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Data
{
    public class NeptuneContext : DbContext
    {
        public NeptuneContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Image> Images { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddTimestamps();
            return await base.SaveChangesAsync();
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries().Where(x =>
                x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var e = (Entity) entity.Entity;
                if (entity.State == EntityState.Added && e.CreatedDate == null)
                {
                    e.CreatedDate = DateTime.Now;
                }
                if (e.UpdatedDate == null)
                {
                    e.UpdatedDate = DateTime.Now;
                }
            }
        }
    }
}