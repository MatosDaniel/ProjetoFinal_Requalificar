using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal.Models
{
    public class FishContext : DbContext 
    {
        public DbSet<Publication> Publications { set; get; }
        public DbSet<User> Users { set; get; }

        public FishContext(DbContextOptions<FishContext> options) : base (options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=library;" + "user=root;password=password");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
            });

            modelBuilder.Entity<Publication>(entity =>
            {
                entity.HasKey(e => e.IdPub);
                entity.Property(e => e.Text).IsRequired();
                entity.HasOne(d => d.User);
            });
        }

    }
}
