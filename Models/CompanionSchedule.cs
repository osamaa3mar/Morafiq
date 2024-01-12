using System.ComponentModel.DataAnnotations;

namespace _Morafiq.Models
{
	public class CompanionSchedule
	{
		[Key]
		public int ScheduleId { get; set; }
		[Required(ErrorMessage = "Companion id is required")]
		public int CompanionId { get; set; }

		public Companion? Companion { get; set; }

		[Required(ErrorMessage = "Schedule date is required")]
		public DateTime StartDate { get; set; }

		[Required(ErrorMessage = "Schedule date is required")]
		public DateTime EndDate { get; set; }

		[Required(ErrorMessage = "User id is required")]
        public string UserId { get; set; }

        public User? User { get; set; }

        [Required]
		public string Status { get; set; }
	}
}
