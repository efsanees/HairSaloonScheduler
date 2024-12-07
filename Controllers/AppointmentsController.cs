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
            ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationId");
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "ConfirmPassword");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,AppointmentDate,Status,OperationId,EmployeeId,UserId")] Appointment appointment)
        {
            if (appointment!=null)
            {
                appointment.AppointmentId = Guid.NewGuid();
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.employees, "EmployeeId", "EmployeeName", appointment.EmployeeId);
            ViewData["OperationId"] = new SelectList(_context.operations, "OperationId", "OperationId", appointment.OperationId);
            ViewData["UserId"] = new SelectList(_context.users, "UserId", "ConfirmPassword", appointment.UserId);
            return View(appointment);
        }
    }
}
