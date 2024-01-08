using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public string UserId { get; set; }
        public User User { get; set; }
        [Range(0, 5)]
        public decimal OrderRate { get; set; }
        [Required]
        [Range(0.1,int.MaxValue)]
        public decimal TotalPrice { get; set; }
        [Required]
        public string Status { get; set; }
    }
}
