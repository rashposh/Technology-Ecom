using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb.Models;

public class OrderModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string OrderCode { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public int Status { get; set; }
    public List<OrderDetails> OrderDetails { get; set; } = [];

    public decimal Total => GetTotal();

    public decimal GetTotal()
    {
        decimal total = 0;
        OrderDetails.ForEach((details) => { total += details.Price; });
        return total;
    }

    public decimal GetTotalOrder()
    {
        decimal total = 0;
        OrderDetails.ForEach((details) => { total += details.Quanlity; });
        return total;
    }

}
