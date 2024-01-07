using System.ComponentModel.DataAnnotations;

namespace _5Dots.Models
{
    public class CompanionImage
    {
        [Key]
        public int ImageId { get; set; }

        [Required(ErrorMessage = "Product ID is required")]
        public int CompanionId { get; set; }


        public Companion Companion { get; set; }

        [Required(ErrorMessage = "Image name is required")]
        public string ImageName { get; set; }

        [Required(ErrorMessage = "Content type is required")]
        public string ContentType { get; set; }

        [Required(ErrorMessage = "Image data is required")]
        public byte[] Image { get; set; }
    }
}