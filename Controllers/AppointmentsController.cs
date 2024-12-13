using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;

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
        public async Task<IActionResult> MyAppointments(User User)
        {
            var currentUserId = Guid.Parse(HttpContext.Request.Cookies["UserId"]);

            if (User == null)
            {
                return NotFound();
            }
            var appointments = await _context.appointments
            .Where(x => x.UserId == currentUserId)
            .ToListAsync();

            if (!appointments.Any())
            {
                return NotFound("No appointments found for this user.");
            }

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
                    var userId = HttpContext.Request.Cookies["UserId"];
                    //appointment.UserId = Guid.Parse(userId);
                    //appointment.AppointmentId = Guid.NewGuid();
                    if (!IsEmployeeAvailable(appointment))
                    {
                        throw new Exception("Employee is not available during the selected time.");
                    }
                    appointment.Operation = await _context.operations.FirstOrDefaultAsync(o => o.OperationId == appointment.OperationId);
                    appointment.Employee = await _context.employees.FirstOrDefaultAsync(o => o.EmployeeId == appointment.EmployeeId);
                    _context.Add(appointment);
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
                appointment.AppointmentId = Guid.NewGuid();
                appointment.UserId = Guid.Parse("5AA27358-A02B-4C4B-8D8C-08DD16D120AA");
                var employee = _context.employees.FirstOrDefault(x => x.EmployeeId == appointment.EmployeeId);
                if (employee == null)
                {
                    throw new Exception("Operation not found.");
                }
                var operation = _context.operations.FirstOrDefault(x => x.OperationId == appointment.OperationId);
                if (operation == null)
                {
                    throw new Exception("Employee not found.");
                }
                //var operationEnd = appointment.AppointmentDate.Add(operation.Duration);
                //var appointmentTime = appointment.AppointmentDate.TimeOfDay;

                //if (appointmentTime < employee.WorkStart || appointmentTime > employee.WorkEnd)
                //{
                //    return false;
                //}
                //var hasConflict = _context.appointments
                //    .Join(
                //        _context.operations,
                //        a => a.OperationId,
                //        o => o.OperationId,
                //        (a, o) => new { Appointment = a, Operation = o }
                //    )
                //    .AsEnumerable() 
                //    .Any(a => a.Appointment.EmployeeId == appointment.EmployeeId &&
                //        (
                //            appointment.AppointmentDate >= a.Appointment.AppointmentDate &&
                //            appointment.AppointmentDate < a.Appointment.AppointmentDate.Add(a.Operation.Duration) ||
                //            operationEnd > a.Appointment.AppointmentDate &&
                //            operationEnd <= a.Appointment.AppointmentDate.Add(a.Operation.Duration) ||
                //            appointment.AppointmentDate < a.Appointment.AppointmentDate &&
                //            operationEnd > a.Appointment.AppointmentDate
                //        )
                //    );

                //return !hasConflict;
                var appointmentStartTime = TimeSpan.FromHours(appointment.AppointmentDate.Hour);

                var IsAnyAppointment = _context.availabilities
                    .FirstOrDefault(x => x.EmployeeId == appointment.EmployeeId &&
                                         x.Date == appointment.AppointmentDate.Date &&
                                         x.StartTime == appointmentStartTime);
            }
            throw new Exception("Appointment not found.");
        }
    }
}
