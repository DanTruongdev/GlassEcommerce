using GlassECommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GlassECommerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<WarehouseLog> WarehousLogs { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ModelAttachment> ModelAttachments { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CollectionProduct> CollectionProducts { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("User");
            builder.Entity<IdentityRole>().ToTable("Role");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRole");
            builder.Entity<CollectionProduct>().HasKey(c => new { c.ProductId, c.CollectionId });
            SeedRole(builder);
            SeedUnit(builder);
            SeedCategory(builder);
            SeedColor(builder);
            SeedCollection(builder);
            SeedProduct(builder);
            SeedModel(builder);

        }

        private void SeedCollection(ModelBuilder builder)
        {
            builder.Entity<Collection>().HasData(
                 new Collection
                 {
                     CollectionId = 1,
                     CollectionName = "Bộ dụng cụ 1",
                     Description = "Bộ dụng cụ xử lí kính 2 trong 1"
                 }

             );
        }
        private void SeedRole(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
              new IdentityRole() { Id = "1", Name = "CUSTOMER", ConcurrencyStamp = "1", NormalizedName = "CUSTOMER" },
              new IdentityRole() { Id = "2", Name = "ADMIN", ConcurrencyStamp = "2", NormalizedName = "ADMIN" }
            );
        }
        private void SeedUnit(ModelBuilder builder)
        {
            builder.Entity<Unit>().HasData(
                new Unit
                {
                    UnitId = 1,
                    UnitName = "Cái"
                },
               new Unit
               {
                   UnitId = 2,
                   UnitName = "Thùng"
               },
               new Unit
               {
                   UnitId = 3,
                   UnitName = "Viên"
               },
               new Unit
               {
                   UnitId = 4,
                   UnitName = "Mũi"
               }
            );
        }
        private void SeedCategory(ModelBuilder builder)
        {
            builder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Họa tiết trang trí"
                }
            );
        }
        private void SeedColor(ModelBuilder builder)
        {
            builder.Entity<Color>().HasData(
                new Color
                {
                    ColorId = 1,
                    ColorName = "Xanh"
                },
                new Color
                {
                    ColorId = 2,
                    ColorName = "Vàng"
                },
                new Color
                {
                    ColorId = 3,
                    ColorName = "Đỏ"
                },
                new Color
                {
                    ColorId = 4,
                    ColorName = "Tím"
                }

            );
        }
        private void SeedProduct(ModelBuilder builder)
        {
            builder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    ProductCode = "PR1",
                    ProductName = "Sản phẩm test",
                    CategoryId = 1,
                    VoteStar = 0,
                    Sold = 0
                }

            );
        }
        private void SeedModel(ModelBuilder builder)
        {
            builder.Entity<Model>().HasData(
                new Model
                {
                    ModelId = 1,
                    ProductId = 1,
                    UnitId = 1,
                    ColorId = 1,
                    Price = 500,
                    Available = 0,
                    Description = "Đây là phần mô tả sản phẩm"
                }

            );
        }
    }
}
