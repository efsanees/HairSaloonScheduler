namespace HairSaloonScheduler.Models
{
	public class Employees
	{
		public Guid EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public Operations ExpertiseArea { get; set; }
		public List<Operations> Abilities { get; set; }
		public List<string> AvailabilityHours { get; set; }
		public double Productivity { get; set; }
		public decimal DailyGain { get; set; }

	}
}
