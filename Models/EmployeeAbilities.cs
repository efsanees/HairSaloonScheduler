using System.ComponentModel.DataAnnotations;

namespace HairSaloonScheduler.Models
{
	public class EmployeeAbilities
	{
		[Key]
		public Guid PK { get; set; }
		public Guid EmployeeId { get; set; }
		public Guid OperationId { get; set; }
		public Employees Employee { get; set; }
		public Operations Operation { get; set; }
	}
}
