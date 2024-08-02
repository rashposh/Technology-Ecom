using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb.Models
{
    public class OrderDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public OrderModel Order { get; set; } = new();
        public ProductModel Product { get; set; } = new();
        public decimal Price { get; set; }
        public int Quanlity { get; set; }


    }
}
