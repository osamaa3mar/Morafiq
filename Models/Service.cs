using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Service Name is required")]
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public List<Companion> Companions { get; set; }
        [Required]
        public string ImageName { get; set; }
        [Required]
        public string contentType { get; set; }
        [Required]
        public byte[] Image { get; set; }
    }
}
