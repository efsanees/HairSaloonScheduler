namespace HairSaloonScheduler.Models
{
	public class Appointment
	{
		public Guid AppointmentId { get; set; }
		public Guid UserId { get; set; }
		public Guid EmployeeId { get; set; }
		public Guid OperationId { get; set; }
		public DateTime AppointmentDate { get; set; }
		public string Status { get; set; }
	}
