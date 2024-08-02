using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace DoAnWeb.Models.Repository
{
    public class DataContext : IdentityDbContext<IdentityUser> //tải microsoft.aspnetcore.identity.entityframeworkcore bản 6.0.21 sau đó doubleclick vào IdentityDbContext rồi alt+enter
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<BradModel> Brands { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }


    }


}
