using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace DoAnWeb.Models.ViewModels
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Lam on nhap Username")]
        public string Username { get; set; }

        [DataType(DataType.Password), Required(ErrorMessage = "Lam on nhap password")]
        public string Password { get; set; }
    }

}