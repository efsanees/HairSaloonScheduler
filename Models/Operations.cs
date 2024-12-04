namespace HairSaloonScheduler.Models
{
	public class Operations
	{
		public Guid OperationId { get; set; }
		public string OperationName { get; set; }
		public int Duration { get; set; }
		public decimal Price { get; set; }
	}
}
