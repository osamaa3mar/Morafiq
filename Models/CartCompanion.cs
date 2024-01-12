

using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class CartCompanion
    {
        [Key]
        public int CartId { get; set; }
        public Cart Cart{ get; set; }
        [Required(ErrorMessage = "Date is required")]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        [Key]
        public int CompanionId { get; set; }
        public Companion Companion { get; set; }

    }
}
