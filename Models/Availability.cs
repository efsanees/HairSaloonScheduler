using System.ComponentModel.DataAnnotations;

namespace HairSaloonScheduler.Models
{
    public class Availability
    {
        [Key]
        public Guid AvailabilityId { get; set; }
        public Employees Employee { get; set; }
        public Guid EmployeeId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
