using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class Companion
    {
        [Key]
        public int CompanionId { get; set; }

        [Required(ErrorMessage = "Companion name is required")]
        public string CompanionName { get; set; }

        [Required(ErrorMessage = "Companion Description is required")]
        [StringLength(255, ErrorMessage = "Companion description cannot exceed 255 characters")]
        public string CompanionDescription { get; set; }

        [Required(ErrorMessage = "Companion price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Companion price must be greater than 0")]
        public decimal CompanionPrice { get; set; }

        [Required(ErrorMessage = "Companion quantity in stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Companion quantity in stock must be greater than or equal to 0")]
        public int CompanionQuantityStock { get; set; }

        [Range(0, 100, ErrorMessage = "Companion sale percentage must be between 0 and 100")]
        public decimal CompanionSale { get; set; }

        [Required(ErrorMessage = "Service ID is required")]
        public int ServiceId { get; set; }

        public Service ? Service { get; set; }

        [Required]
        public string ImageName { get; set; }
        [Required]
        public string contentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
        public List<Review>? Reviews { get; set; }


        //public List<Order> Orders { get; set; }
    }
}