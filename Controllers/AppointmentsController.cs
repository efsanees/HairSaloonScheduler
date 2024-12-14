using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using System.Security.Claims;

namespace HairSaloonScheduler.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly MyDbContext _context;

        public AppointmentsController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.appointments.Include(a => a.Employee).Include(a => a.Operation).Include(a => a.User);
            return View(await myDbContext.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.employees, "EmployeeId", "EmployeeName");
            ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationName");
            return View();
        }

		[HttpGet]
		public async Task<IActionResult> MyAppointments()
		{
			// Kullanıcı kimliği (UserId) alınıyor.
			var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
			if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
			{
				// Eğer kullanıcı giriş yapmamışsa, giriş sayfasına yönlendirilir.
				return RedirectToAction("Login", "Login");
			}

			if (!Guid.TryParse(userIdClaim.Value, out var currentUserId))
			{
				// Kullanıcı ID'si geçersizse hata döndürülür.
				return BadRequest("Geçersiz kullanıcı oturumu.");
			}

			// Kullanıcının randevuları veri tabanından çekiliyor.
			var appointments = await _context.appointments
				.Where(x => x.UserId == currentUserId).
				Include(a => a.Employee).Include(a => a.Operation).Include(a => a.User)
				.ToListAsync();

			if (!appointments.Any())
			{
				// Kullanıcıya ait randevu bulunamazsa bilgilendirme yapılır.
				return View("NoAppointments"); // NoAppointments adında bir bilgilendirme sayfası oluşturabilirsiniz.
			}

			// Kullanıcının randevuları view'a gönderilir.
			return View(appointments);
		}



		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentDate,OperationId,EmployeeId")] Appointment appointment)
        {
            try
            {
                if (appointment != null)
                {
                    appointment.AppointmentId = Guid.NewGuid();
                    var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim == null)
                    {
                        throw new Exception("User ID not found in cookie.");
                    }
                    appointment.UserId = Guid.Parse(userIdClaim.Value);

                    var employee = _context.employees.FirstOrDefault(x => x.EmployeeId == appointment.EmployeeId);
                    if (employee == null)
                    {
                        throw new Exception("Employee not found.");
                    }

                    var operation = _context.operations.FirstOrDefault(x => x.OperationId == appointment.OperationId);
                    if (operation == null)
                    {
                        throw new Exception("Operation not found.");
                    }
                    appointment.Operation = operation;
                    appointment.Employee= employee;
                    if (!IsEmployeeAvailable(appointment))
                    {
                        throw new Exception("Employee is not available during the selected time.");
                    }
                    if(appointment.OperationId!= Guid.Empty && appointment.EmployeeId!= Guid.Empty)
                    {
                        appointment.Operation = await _context.operations.FirstOrDefaultAsync(o => o.OperationId == appointment.OperationId);
                        appointment.Employee = await _context.employees.FirstOrDefaultAsync(o => o.EmployeeId == appointment.EmployeeId);
                        var availability = new Availability
                        {
                            AvailabilityId = Guid.NewGuid(),
                            EmployeeId = appointment.EmployeeId,
                            Date = appointment.AppointmentDate.Date,
                            StartTime = appointment.AppointmentDate.TimeOfDay,
                            EndTime = appointment.AppointmentDate.Add(appointment.Operation.Duration).TimeOfDay
                        };
                        _context.Add(availability);
                        availability.Employee = _context.employees.FirstOrDefault(x => x.EmployeeId == availability.EmployeeId);
                        _context.Add(appointment);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            ViewData["EmployeeId"] = new SelectList(_context.employees, "EmployeeId", "EmployeeName", appointment.EmployeeId);
            ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationName", appointment.OperationId);
            return View(appointment);
        }

        private bool IsEmployeeAvailable([Bind("AppointmentDate,OperationId,EmployeeId")] Appointment appointment)
        {
            if (appointment != null)
            {
               

                var appointmentStartTime = TimeSpan.FromHours(appointment.AppointmentDate.Hour);
                var appointmentEndTime = appointment.AppointmentDate.Add(appointment.Operation.Duration).TimeOfDay;

                if(appointmentStartTime < appointment.Employee.WorkStart && appointmentEndTime>appointment.Employee.WorkEnd ) {
                    return false;
                }
                var IsAnyAppointment = _context.availabilities
                    .FirstOrDefault(x => x.EmployeeId == appointment.EmployeeId &&
                                         x.Date == appointment.AppointmentDate.Date &&
                                         x.StartTime == appointmentStartTime &&
                                         x.EndTime == appointmentEndTime);
                if (IsAnyAppointment != null)
                {
                    return false;
                }
                return true;
            }
            throw new Exception("Appointment not found.");
        }
    }
}
