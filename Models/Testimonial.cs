using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
    public class Testimonial
    {
        public int TestimonialId { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required(ErrorMessage = "Testimonial status is required")]
        public string TestimonialStatus { get; set; }

        [Required(ErrorMessage = "Testimonial message is required")]
        public string TestimonialMessage { get; set; }
    }
}
