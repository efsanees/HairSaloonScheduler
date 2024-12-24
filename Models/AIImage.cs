namespace HairSaloonScheduler.Models
{
	public class AIImage
	{
		public Data data { get; set; }
		public class Data
		{
			public Image image { get; set; }
		}

		public class Image
		{
			public string Type { get; set; }
			public string Description { get; set; }
		}
	}
}
