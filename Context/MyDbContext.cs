using HairSaloonScheduler.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;
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

            modelBuilder.Entity<Statistics>()
                .Property(s => s.Gain)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Employees>()
            .HasOne(e => e.ExpertiseArea)
            .WithMany()
            .HasForeignKey(e => e.ExpertiseAreaId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Operation)
            .WithMany()
            .HasForeignKey(e => e.OperationId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointment>()
            .HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Statistics>()
            .HasOne(e => e.Employee)
            .WithMany()
            .HasForeignKey(e => e.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Employees>()
            .HasMany(e => e.Abilities)
            .WithMany(e => e.Employees);

            modelBuilder.Entity<Employees>()
            .HasMany(e => e.Abilities)
            .WithMany(e => e.Employees)
            .UsingEntity(
                "EmployeeAbilities",
                l => l.HasOne(typeof(Operations)).WithMany().HasForeignKey("OperationId").HasPrincipalKey(nameof(Operations.OperationId)),
                r => r.HasOne(typeof(Employees)).WithMany().HasForeignKey("EmployeeId").HasPrincipalKey(nameof(Employees.EmployeeId)),
                j => j.HasKey("OperationId", "EmployeeId"));

            modelBuilder.Entity<EmployeeAbilities>()
            .HasOne(ea => ea.Employee)
            .WithMany(e => e.EmployeeAbilities)
            .HasForeignKey(ea => ea.EmployeeId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<EmployeeAbilities>()
                .HasOne(ea => ea.Operation)
                .WithMany(o => o.EmployeeAbilities)
                .HasForeignKey(ea => ea.OperationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);
		}

        public DbSet<Appointment> appointments { get; set; }
        public DbSet<Employees> employees { get; set; }
        public DbSet<Operations> operations { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<Admin> admins { get; set; }
        public DbSet<Availability> availabilities { get; set; }
        public DbSet<Statistics> statistics { get; set; }
        public DbSet<EmployeeAbilities> employeeAbilities { get; set; }
    }
}
