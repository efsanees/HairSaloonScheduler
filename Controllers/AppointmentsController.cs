using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
            var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                return RedirectToAction("Login", "Login");
            }

            if (!Guid.TryParse(userIdClaim.Value, out var currentUserId))
            {
                TempData["ErrorMessage"] = "Geçersiz kullanıcı oturumu.";
                return RedirectToAction("Index");
            }

            if (HttpContext.User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Appointments");
            }

            var appointments = await _context.appointments
                .Where(x => x.UserId == currentUserId)
                .Include(a => a.Employee)
                .Include(a => a.Operation)
                .Include(a => a.User)
                .ToListAsync();

            if (!appointments.Any())
            {
                TempData["ErrorMessage"] = "No Appointments.";
                return View();
            }

            return View(appointments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentDate,OperationId,EmployeeId")] Appointment appointment)
        {
            if (appointment==null)
            {
                ViewData["EmployeeId"] = new SelectList(_context.employees, "EmployeeId", "EmployeeName", appointment.EmployeeId);
                ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationName", appointment.OperationId);
                return View(appointment);
            }

            try
            {
                if (appointment != null)
                {
                    appointment.AppointmentId = Guid.NewGuid();
                    var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                    if (userIdClaim == null)
                    {
                        TempData["ErrorMessage"] = "Please Login First";
                        return RedirectToAction("Login", "Login");
                    }
                    appointment.UserId = Guid.Parse(userIdClaim.Value);

                    var employee = _context.employees.FirstOrDefault(x => x.EmployeeId == appointment.EmployeeId);
                    if (employee == null)
                    {
                        TempData["ErrorMessage"] = "Employee not found.";
                        return RedirectToAction("Create");
                    }

                    var operation = _context.operations.FirstOrDefault(x => x.OperationId == appointment.OperationId);
                    if (operation == null)
                    {
                        TempData["ErrorMessage"] = "Operation not found.";
                        return RedirectToAction("Create");
                    }

                    appointment.Operation = operation;
                    appointment.Employee = employee;

                    if (!IsEmployeeAvailable(appointment))
                    {
                        TempData["ErrorMessage"] = "Employee is not available during the selected time.";
                        return RedirectToAction("Create");
                    }

                    var availability = new Availability
                    {
                        AvailabilityId = Guid.NewGuid(),
                        EmployeeId = appointment.EmployeeId,
                        Date = appointment.AppointmentDate.Date,
                        StartTime = appointment.AppointmentDate.TimeOfDay,
                        EndTime = appointment.AppointmentDate.Add(appointment.Operation.Duration).TimeOfDay
                    };
                    availability.Employee = _context.employees.FirstOrDefault(x => x.EmployeeId == availability.EmployeeId);
                    _context.Add(availability);
                    _context.Add(appointment);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Appointment created successfully!";
                    return RedirectToAction(nameof(Create));
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Create");
            }

            ViewData["EmployeeId"] = new SelectList(_context.employees, "EmployeeId", "EmployeeName", appointment.EmployeeId);
            ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationName", appointment.OperationId);
            return View(appointment);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(Guid appointmentId)
        {
            try
            {
                var appointment = await _context.appointments
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction(nameof(Index));
                }

                appointment.Status = AppointmentStatus.Approved.ToString();
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Appointment approved successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while approving the appointment: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid appointmentId)
        {
            try
            {
                var appointment = await _context.appointments
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                {
                    TempData["ErrorMessage"] = "Appointment not found.";
                    return RedirectToAction(nameof(Index));
                }

                appointment.Status = AppointmentStatus.Canceled.ToString();
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Appointment canceled successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while canceling the appointment: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool IsEmployeeAvailable([Bind("AppointmentDate,OperationId,EmployeeId")] Appointment appointment)
        {
            if (appointment != null)
            {
                var appointmentStartTime = TimeSpan.FromHours(appointment.AppointmentDate.Hour);
                var appointmentEndTime = appointment.AppointmentDate.Add(appointment.Operation.Duration).TimeOfDay;

                if (appointmentStartTime < appointment.Employee.WorkStart || appointmentEndTime > appointment.Employee.WorkEnd)
                {
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
