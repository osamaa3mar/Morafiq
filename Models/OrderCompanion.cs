using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class OrderCompanion
    {
        [Key]
        public int OrderId { get; set; }


        public Order Order { get; set; }

        [Key]
        public int CompanionId { get; set; }

        public Companion Companion { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int CompanionQuantity { get; set; }
    }
}