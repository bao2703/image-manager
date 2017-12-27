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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            base.OnModelCreating(modelBuilder);
        }

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
                x.Entity is Entity && x.State == EntityState.Added);

            foreach (var entity in entities)
            {
                var e = (Entity) entity.Entity;
                e.DateCreated = DateTime.Now;
            }
        }
    }
}