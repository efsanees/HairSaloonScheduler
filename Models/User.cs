﻿namespace HairSaloonScheduler.Models
{
	public class User
	{
		public Guid UserId { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public string Password { get; set; }
		public string Role { get; set; }
	}
}
