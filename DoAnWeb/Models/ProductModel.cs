using DoAnWeb.Models.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb.Models
{
    public class ProductModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MinLength(4, ErrorMessage = "yêu cầu nhập tên sản phẩm ")]
        public string Name { get; set; }
        public string Slug { get; set; }
        [Required, MinLength(4, ErrorMessage = "yêu cầu nhập mô tả sản phẩm")]
        public string Description { get; set; }

        [Required(ErrorMessage = "yêu cầu nhập giá sản phẩm ")]
        public decimal Price { get; set; }
        public CategoryModel Category { get; set; }
        public BradModel Brad { get; set; }
        public string Image { get; set; } = "noimage.jpg";

        [NotMapped]
        [FileExtension]
        public IFormFile? ImageUpload { get; set; }
    }
}
