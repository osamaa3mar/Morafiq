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
		public DateTime ScheduleDate { get; set; }

		[Required]
		public string Status { get; set; }
	}
}
