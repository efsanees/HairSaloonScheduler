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
    public class EmployeesController : Controller
    {
        private readonly MyDbContext _context;

        public EmployeesController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var myDbContext = _context.employees.Include(e => e.ExpertiseArea);
            return View(await myDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employees = await _context.employees
                .Include(e => e.ExpertiseArea)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        public IActionResult Create()
        {
            ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeName,ExpertiseAreaId,WorkStart,WorkEnd")] Employees employees)
        {
            if (employees!=null)
            {
                employees.EmployeeId = Guid.NewGuid();
                if (employees.ExpertiseAreaId != null)
                {
                    employees.ExpertiseArea = await _context.operations
                        .FirstOrDefaultAsync(o => o.OperationId == employees.ExpertiseAreaId);
                };
                _context.Add(employees);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employees = await _context.employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }

            ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
            return View(employees);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employees employees)
        {
            if (employees==null)
            {
                return NotFound();
            }

            try
            {
                _context.Update(employees);
                    if (employees.ExpertiseAreaId != null)
                    {
                        employees.ExpertiseArea = await _context.operations
                            .FirstOrDefaultAsync(o => o.OperationId == employees.ExpertiseAreaId);
                    }

                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeesExists(employees.EmployeeId))
                {
                    return NotFound();
                }
                else
                {
                    ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
                    return View(employees);
                    throw;
                }
            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.employees == null)
            {
                return NotFound();
            }

            var employees = await _context.employees
                .Include(e => e.ExpertiseArea)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Employees employee)
        {
            if (_context.employees == null)
            {
                return Problem("Entity set 'MyDbContext.employees'  is null.");
            }
            if (employee != null)
            {
                _context.employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(Guid id)
        {
            return _context.employees.Any(e => e.EmployeeId == id);
        }
    }
}
