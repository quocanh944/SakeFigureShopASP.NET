using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SakeFigureShop.Domains;

namespace SakeFigureShop.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Film> Films { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Brand>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Brand)
                .HasForeignKey(e => e.BrandId)
                .IsRequired(false);
            builder.Entity<Film>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Film)
                .HasForeignKey(e => e.FilmId)
                .IsRequired(false);
            builder.Entity<Product>()
                .HasMany(e => e.Medias)
                .WithOne(e => e.Product)
                .HasForeignKey(e => e.ProductId)
                .IsRequired();
            builder.Entity<User>()
                .HasMany(e => e.CartItems)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            builder.Entity<CartItem>()
                .HasOne(e => e.Product);

            builder
                .Entity<Media>()
                .Property(e => e.MediaType)
                .HasConversion(v => v.ToString(), v => (MediaType)Enum.Parse(typeof(MediaType), v));
        }
    }
}