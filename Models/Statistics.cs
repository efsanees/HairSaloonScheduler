using System.ComponentModel.DataAnnotations;

namespace HairSaloonScheduler.Models
{
    public class Statistics
    {
        [Key]
        public Guid StatisticId { get; set; }
        public Guid EmployeeId { get; set;}
        public double WorkHour { get; set;}
        public decimal Gain { get; set; }
        public DateTime Day { get; set; }
    }
}
