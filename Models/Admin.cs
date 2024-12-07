using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSaloonScheduler.Models
{
	public class Admin
	{
		[Key]
		public Guid AdminId { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(100)")]
		[EmailAddress]
		public string AdminMail { get; set; }
		[Required]
		[Column(TypeName = "nvarchar(100)")]
        public string Password { get; set; }

	}
}
