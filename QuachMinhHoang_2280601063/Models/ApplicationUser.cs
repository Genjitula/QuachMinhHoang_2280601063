using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuachMinhHoang_2280601063.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string Sex { get; set; }
        public string? Age { get; set; }
    }
}
