using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is required")]
        public string ?FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string ?LastName { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public string ?Role { get; set; }

        [StringLength(255, ErrorMessage = "Image name length must be less than or equal to 255 characters")]
        public string ?ImageName { get; set; }

        [StringLength(10, ErrorMessage = "Content type length must be less than or equal to 50 characters")]
        public string ?ContentType { get; set; }

        [Required(ErrorMessage = "Image is required")]
        public byte[] ?Image { get; set; }
    }
}
