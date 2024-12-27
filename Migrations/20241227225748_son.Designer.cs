﻿// <auto-generated />
using System;
using HairSaloonScheduler.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HairSaloonScheduler.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20241227225748_son")]
    partial class son
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.35")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EmployeeAbilities", b =>
                {
                    b.Property<Guid>("OperationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("OperationId", "EmployeeId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeAbilities");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Admin", b =>
                {
                    b.Property<Guid>("AdminId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdminMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AdminId");

                    b.ToTable("admins");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Appointment", b =>
                {
                    b.Property<Guid>("AppointmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OperationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AppointmentId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OperationId");

                    b.HasIndex("UserId");

                    b.ToTable("appointments");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Availability", b =>
                {
                    b.Property<Guid>("AvailabilityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("EndTime")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("StartTime")
                        .HasColumnType("time");

                    b.HasKey("AvailabilityId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("availabilities");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.EmployeeAbilities", b =>
                {
                    b.Property<Guid>("PK")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OperationId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PK");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("OperationId");

                    b.ToTable("employeeAbilities");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Employees", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DailyGain")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("EmployeeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ExpertiseAreaId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<double>("Productivity")
                        .HasColumnType("float");

                    b.Property<TimeSpan>("WorkEnd")
                        .HasColumnType("time");

                    b.Property<TimeSpan>("WorkStart")
                        .HasColumnType("time");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ExpertiseAreaId");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Operations", b =>
                {
                    b.Property<Guid>("OperationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("OperationName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("OperationId");

                    b.ToTable("operations");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Statistics", b =>
                {
                    b.Property<Guid>("StatisticId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Day")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Gain")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Productivity")
                        .HasColumnType("float");

                    b.Property<double>("WorkHour")
                        .HasColumnType("float");

                    b.HasKey("StatisticId");

                    b.HasIndex("EmployeeId");

                    b.ToTable("statistics");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserMail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("EmployeeAbilities", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Employees", null)
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HairSaloonScheduler.Models.Operations", null)
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Appointment", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Employees", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HairSaloonScheduler.Models.Operations", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HairSaloonScheduler.Models.User", "User")
                        .WithMany("Appointments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Operation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Availability", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Employees", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.EmployeeAbilities", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Employees", "Employee")
                        .WithMany("EmployeeAbilities")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("HairSaloonScheduler.Models.Operations", "Operation")
                        .WithMany("EmployeeAbilities")
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Operation");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Employees", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Operations", "ExpertiseArea")
                        .WithMany()
                        .HasForeignKey("ExpertiseAreaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ExpertiseArea");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Statistics", b =>
                {
                    b.HasOne("HairSaloonScheduler.Models.Employees", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Employees", b =>
                {
                    b.Navigation("EmployeeAbilities");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.Operations", b =>
                {
                    b.Navigation("EmployeeAbilities");
                });

            modelBuilder.Entity("HairSaloonScheduler.Models.User", b =>
                {
                    b.Navigation("Appointments");
                });
#pragma warning restore 612, 618
        }
    }
}