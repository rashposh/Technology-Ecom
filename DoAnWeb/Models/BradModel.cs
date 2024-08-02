using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnWeb.Models;

public class BradModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required(ErrorMessage = "yêu cầu nhập tên thương hiệu")]
    public string Name { get; set; }
    [Required(ErrorMessage = "yêu cầu nhập mô tả thương hiệu")]
    public string Description { get; set; }
    public string Slug { get; set; }
    public int Status { get; set; }
}
