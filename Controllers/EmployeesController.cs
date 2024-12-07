using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HairSaloonScheduler.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly MyDbContext _context;

        public EmployeeController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["ExpertiseArea"] = new SelectList(await _context.operations.ToListAsync(), "OperationId", "OperationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employees employee)
        {
            if (employee != null)
            {
                var expertiseArea = await _context.operations
                    .FirstOrDefaultAsync(o => o.OperationId == employee.ExpertiseAreaId);

                if (expertiseArea != null)
                {
                    employee.ExpertiseArea = expertiseArea;
                }
                else
                {
                    ViewBag.error("ExpertiseAreaId", "Invalid Expertise Area selected.");
                    ViewData["ExpertiseArea"] = new SelectList(await _context.operations.ToListAsync(), "OperationId", "OperationName");
                    return View(employee);
                }
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return View(employee);
            }

            ViewData["ExpertiseArea"] = new SelectList(await _context.operations.ToListAsync(), "OperationId", "OperationName");
            return View(employee);
        }

    }
}
