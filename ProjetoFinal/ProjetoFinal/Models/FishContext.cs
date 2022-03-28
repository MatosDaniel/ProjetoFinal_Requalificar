using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal.Models
{
    public class FishContext : DbContext 
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<User> Users { get; set; }

        public FishContext(DbContextOptions<FishContext> options) : base (options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;database=fish;" + "user=root;password=password");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
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
