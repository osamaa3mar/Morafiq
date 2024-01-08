

using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class CartCompanion
    {
        [Key]
        public int CartId { get; set; }
        public Cart Cart{ get; set; }
        [Required(ErrorMessage = "Cart Quantity is required")]
        [Range(1,int.MaxValue)]
        public int CompanionQuantity { get; set; }
        [Key]
        public int CompanionId { get; set; }
        public Companion Companion { get; set; }

    }
}
