using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HairSaloonScheduler.Models
{
	public class Operations
	{
		[Key]
		public Guid OperationId { get; set; }
		public string OperationName { get; set; }
		public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
		public ICollection<Employees>? Employees { get; set; }
		public ICollection<EmployeeAbilities> EmployeeAbilities { get; set; }
	}
}
	