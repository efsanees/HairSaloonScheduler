using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HairSaloonScheduler.Models
{
	public class User
	{
		[Key]
		public Guid UserId { get; set; }
        [Required]
        [EmailAddress]
        public string UserMail { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }
	}
}
