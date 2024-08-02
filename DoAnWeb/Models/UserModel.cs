using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DoAnWeb.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lam on nhap Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lam on nhap Email"), EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password), Required(ErrorMessage = "Lam on nhap password")]
        public string Password { get; set; }
    }
}
