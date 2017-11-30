using System;
using System.Linq;
using System.Threading.Tasks;
using ImageManager.Data.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ImageManager.Data
{
    public class NeptuneContext : IdentityDbContext<User>
    {
        public NeptuneContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Category> Categories { get; set; }

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
                x.Entity is ITimestampEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var e = (ITimestampEntity) entity.Entity;
                if (entity.State == EntityState.Added)
                    e.DateCreated = DateTime.Now;
                e.DateModified = DateTime.Now;
            }
        }
    }
}