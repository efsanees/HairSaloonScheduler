namespace HairSaloonScheduler.Models
{
	public class APIResponse
	{
		public string request_id { get; set; }
		public string log_id { get; set; }
		public int error_code { get; set; }
		public ErrorDetail error_detail { get; set; }
		public string error_msg { get; set; }
		public Data data { get; set; }

		public class ErrorDetail
		{
			public int status_code { get; set; }
			public string code { get; set; }
			public string code_message { get; set; }
			public string message { get; set; }
		}

		public class Data
		{
			public string image { get; set; }
		}
	}

}
