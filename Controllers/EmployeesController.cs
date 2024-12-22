using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Authorization;

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
			if (_context.employees == null)
			{
				TempData["Error"] = "Employee list could not be loaded.";
				return RedirectToAction("Error");
			}

			var employees = await _context.employees.Include(e => e.ExpertiseArea).Include(e => e.EmployeeAbilities).ThenInclude(ea => ea.Operation).ToListAsync();
			if (employees == null || !employees.Any())
			{
				ViewBag.Message = "No employees found.";
				return View();
			}

			return View(employees);
		}

		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null)
			{
				TempData["Error"] = "Employee ID is missing.";
				return RedirectToAction(nameof(Index));
			}

			var employees = await _context.employees
				.Include(e => e.ExpertiseArea)
				.Include(e => e.EmployeeAbilities)
				.ThenInclude(ea => ea.Operation)
				.FirstOrDefaultAsync(m => m.EmployeeId == id);

			if (employees == null)
			{
				TempData["Error"] = "Employee not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(employees);
		}

		[Authorize(Roles = "Admin")]
		public IActionResult Create()
		{
			if (_context.operations == null)
			{
				TempData["Error"] = "Operation list could not be loaded.";
				return RedirectToAction(nameof(Index));
			}

			ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName");
			ViewData["EmployeeAbilities"] = _context.operations
			.Select(o => new SelectListItem
			{
				Value = o.OperationId.ToString(),
				Text = o.OperationName
			})
			.ToList();

			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EmployeeName,ExpertiseAreaId,WorkStart,WorkEnd")] Employees employees,IEnumerable<Guid> selectedAbilities)
		{
			if (employees==null)
			{
				ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId).ToList();
				ViewData["EmployeeAbilities"] = _context.operations
				.Select(o => new SelectListItem
				{
					Value = o.OperationId.ToString(),
					Text = o.OperationName
				})
				.ToList();
				TempData["Error"] = "Invalid employee data.";
				return RedirectToAction(nameof(Index));
			}

			employees.EmployeeId = Guid.NewGuid();
			if (employees.ExpertiseAreaId != null)
			{
				employees.ExpertiseArea = await _context.operations
					.FirstOrDefaultAsync(o => o.OperationId == employees.ExpertiseAreaId);
			}

			employees.EmployeeAbilities = new List<EmployeeAbilities>();
			foreach (var abilityId in selectedAbilities)
			{
				var operation = await _context.operations.FindAsync(abilityId);
				if (operation != null)
				{
					employees.EmployeeAbilities.Add(new EmployeeAbilities
					{
						EmployeeId = employees.EmployeeId,
						OperationId = operation.OperationId,
						Employee = employees,
						Operation = operation
					});
				}
			}


			try
			{
				_context.Add(employees);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"An error occurred: {ex.Message}";
				ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
				ViewData["EmployeeAbilities"] = _context.operations
				.Select(o => new SelectListItem
				{
					Value = o.OperationId.ToString(),
					Text = o.OperationName
				})
				.ToList();
				return View(employees);
			}
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null)
			{
				TempData["Error"] = "Employee ID is missing.";
				return RedirectToAction(nameof(Index));
			}

			var employees = await _context.employees
				.Include(e=> e.ExpertiseArea)
			   .Include(e => e.EmployeeAbilities)
			   .ThenInclude(ea => ea.Operation)
			   .FirstOrDefaultAsync(e => e.EmployeeId == id);

			if (employees == null)
			{
				TempData["Error"] = "Employee not found.";
				return RedirectToAction(nameof(Index));
			}

			ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);

            ViewData["EmployeeAbilities"] = _context.operations
                .Select(o => new SelectListItem
                {
                    Value = o.OperationId.ToString(),
                    Text = o.OperationName,
                    Selected = _context.employeeAbilities.Any(ea => ea.OperationId == o.OperationId)
                })
                .ToList();

            return View(employees);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit([Bind("EmployeeId,EmployeeName,ExpertiseAreaId,WorkStart,WorkEnd")] Employees employees, IEnumerable<Guid> selectedAbilities)
        {

            if (employees==null)
            {
                ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
                ViewData["EmployeeAbilities"] = _context.operations
                .Select(o => new SelectListItem
                {
                    Value = o.OperationId.ToString(),
                    Text = o.OperationName,
                    Selected = _context.employeeAbilities.Any(ea => ea.OperationId == o.OperationId)
                })
                .ToList();
                return View(employees);
            }

			var existingEmployee = await _context.employees
			.Include(o => o.ExpertiseArea)
			.Include(e => e.EmployeeAbilities)
			.ThenInclude(ea => ea.Operation)
			.FirstOrDefaultAsync(e => e.EmployeeId == employees.EmployeeId);

			if (existingEmployee == null)
            {
                TempData["Error"] = "Employee not found.";
                return RedirectToAction(nameof(Index));
            }

            existingEmployee.EmployeeName = employees.EmployeeName;
            existingEmployee.ExpertiseAreaId = employees.ExpertiseAreaId;
            existingEmployee.WorkStart = employees.WorkStart;
            existingEmployee.WorkEnd = employees.WorkEnd;

            _context.employeeAbilities.RemoveRange(existingEmployee.EmployeeAbilities);
            foreach (var abilityId in selectedAbilities)
			{
				var operation = await _context.operations.FindAsync(abilityId);
				if (operation != null)
				{
					existingEmployee.EmployeeAbilities.Add(new EmployeeAbilities
					{
						EmployeeId = existingEmployee.EmployeeId,
						OperationId = operation.OperationId,
						Employee = existingEmployee,
						Operation = operation
					});
				}
			}

			await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null)
			{
				TempData["Error"] = "Employee ID is missing.";
				return RedirectToAction(nameof(Index));
			}

			var employees = await _context.employees
				.Include(e => e.ExpertiseArea)
				.FirstOrDefaultAsync(m => m.EmployeeId == id);
			if (employees == null)
			{
				TempData["Error"] = "Employee not found.";
				return RedirectToAction(nameof(Index));
			}

			return View(employees);
		}

		[HttpPost, ActionName("Delete")]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Employees employees)
		{
			if (_context.employees == null)
			{
				TempData["Error"] = "Employee entity is null.";
				return RedirectToAction(nameof(Index));
			}

			var employee = await _context.employees.FindAsync(employees.EmployeeId);
			if (employee == null)
			{
				TempData["Error"] = "Employee not found.";
				return RedirectToAction(nameof(Index));
			}

			try
			{
				_context.employees.Remove(employee);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"An error occurred: {ex.Message}";
				return RedirectToAction(nameof(Index));
			}
		}

		private bool EmployeesExists(Guid id)
		{
			return _context.employees?.Any(e => e.EmployeeId == id) ?? false;
		}
	}
}
