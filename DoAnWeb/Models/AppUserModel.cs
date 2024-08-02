using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;

namespace DoAnWeb.Models
{
    public class AppUserModel : IdentityUser
    {
        public string Occupation { get; set; }
    }
}