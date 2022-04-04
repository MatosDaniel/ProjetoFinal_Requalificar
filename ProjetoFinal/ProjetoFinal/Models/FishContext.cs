using Microsoft.EntityFrameworkCore;

namespace ProjetoFinal.Models
{
    //Database connection
    public class FishContext : DbContext 
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<User> Users { get; set; }

        public FishContext(DbContextOptions<FishContext> options) : base (options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=socialfish.mysql.database.azure.com;database=fish;" + "user=fishadmin@socialfish;password=Password123");
        }

        //Link para aceder ao site: fishsocial.azurewebsites.net

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Table User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.Property(e => e.Password).IsRequired();
            });

            //Table Publication
            modelBuilder.Entity<Publication>(entity =>
            {
                entity.HasKey(e => e.IdPub);
                entity.Property(e => e.Text).IsRequired();
                entity.HasOne(d => d.User);
            });
        }

    }
}
