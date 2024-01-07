using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public int TotalQuantity { get; set; }
    }
}
