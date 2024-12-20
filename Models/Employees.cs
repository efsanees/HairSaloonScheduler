using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSaloonScheduler.Models
{
    public class Employees
    {
        [Key]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee Name is required.")]
        public string EmployeeName { get; set; }

        public Guid ExpertiseAreaId { get; set; }
        public Operations ExpertiseArea { get; set; }

        [Range(0, 100, ErrorMessage = "Productivity must be between 0 and 100.")]
        public double Productivity { get; set; } = 0;

        [Range(0, double.MaxValue, ErrorMessage = "Daily gain must be a positive number.")]
        public decimal DailyGain { get; set; } = 0m;
        public List<Operations> Abilities { get; set; }

        [Required(ErrorMessage = "Work start time is required.")]
        public TimeSpan WorkStart { get; set; }

        [Required(ErrorMessage = "Work end time is required.")]
        public TimeSpan WorkEnd { get; set; }

    }
}
