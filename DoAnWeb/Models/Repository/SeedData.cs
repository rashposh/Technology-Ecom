using DoAnWeb.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DoAnWeb.Models.Repository
{
    public class SeedData
    {
        public static void SeedingData(DataContext _context)
        {
            _context.Database.Migrate();
            if (!_context.Products.Any())
            {
                BradModel Apple = new BradModel { Name = "Apple", Slug = "apple", Description = "Apple is large Brand in the world", Status = 1 };
                BradModel Samsung = new BradModel { Name = "Samsung", Slug = "samsung", Description = "samsung is large Brand in the world", Status = 1 };
                CategoryModel Dell = new CategoryModel { Name = "Dell", Slug = "dell", Description = "Dell is large Brand in the world", Status = 1 };
                var Asus = new CategoryModel { Name = "Asus", Slug = "asus", Description = "Asus is large Brand in the world", Status = 1 };
                _context.Products.AddRange(
                    new ProductModel { Name = "Dell", Slug = "dell", Description = "dell is company", Image = "a.jpg", Category = Dell, Brad = Samsung, Price = 1000 },

                    new ProductModel { Name = "Asus", Slug = "asus", Description = "Asus is suc", Image = "a.jpg", Category = Asus, Brad = Apple, Price = 1000 }
                );
                _context.SaveChanges();
            }
        }
    }
}
