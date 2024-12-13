using System.ComponentModel.DataAnnotations;

namespace HairSaloonScheduler.Models
{
    public class Appointment
    {
        [Key]
        public Guid AppointmentId { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = (AppointmentStatus.Waiting).ToString();

        [Required]
        public Guid OperationId { get; set; }
        public Operations Operation { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
        public Employees Employee { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }

    public enum AppointmentStatus
    {
        Waiting,
        Approved,
        Canceled
    }
}