using HairSaloonScheduler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace HairSaloonScheduler.Context
{
	public class MyDbContext : DbContext
	{
		public MyDbContext(DbContextOptions options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
            var timeOnlyConverter = new ValueConverter<TimeOnly, TimeSpan>(
                t => t.ToTimeSpan(),
                ts => TimeOnly.FromTimeSpan(ts));

            modelBuilder.Entity<Appointment>()
				.HasOne(a => a.Operation)
				.WithMany()
				.HasForeignKey(a => a.OperationId)
				.OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Employees>()
				.Property(e => e.DailyGain)
				.HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Operations>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18,2)");

        }

		public DbSet<Appointment> appointments { get; set; }
		public DbSet<Employees> employees { get; set; }
		public DbSet<Operations> operations { get; set; }
		public DbSet<User> users { get; set; }
		public DbSet<Admin> admins { get; set; }
        public DbSet<Availability> availabilities { get; set; }
        public DbSet<Statistics> statistics { get; set; }
    }
}
