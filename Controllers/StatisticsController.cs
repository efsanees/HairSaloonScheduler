using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Models;
using HairSaloonScheduler.Context;

namespace HairSaloonScheduler.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly MyDbContext _context;

        public StatisticsController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetStatistics(Guid employeeId, DateTime day)
        {
            var employees = await _context.employees.ToListAsync();
            ViewData["Employees"] = employees;

            if (employeeId == Guid.Empty)
            {
                TempData["ErrorMessage"] = "Çalışan ve tarih seçmelisiniz!";
                return View();
            }

            var availability = await _context.availabilities
                .Where(a => a.EmployeeId == employeeId && a.Date == day.Date)
                .ToListAsync();
            double workHour = availability.Sum(a => (a.EndTime - a.StartTime).TotalHours);

            var appointments = await _context.appointments
            .Where(a => a.EmployeeId == employeeId && a.AppointmentDate.Date == day.Date)
            .Include(a => a.Operation)
            .ToListAsync();


            decimal gain = 0;
            foreach (var a in appointments)
            {
                if (a.Operation != null)
                {
                    gain += a.Operation.Price;
                }
            }

            var statistics = new Statistics
            {
                EmployeeId = employeeId,
                Day = day,
                WorkHour = workHour,
                Gain = gain
            };

            return View(statistics);
        }

    }
}
