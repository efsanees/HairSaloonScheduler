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

			var employees = await _context.employees.Include(e => e.ExpertiseArea).ToListAsync();
			if (employees == null || !employees.Any())
			{
				ViewBag.Message = "No employees found.";
				return View("Empty");
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
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("EmployeeName,ExpertiseAreaId,WorkStart,WorkEnd")] Employees employees)
		{
			if (!ModelState.IsValid)
			{
				ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
				return View(employees);
			}

			if (employees == null)
			{
				TempData["Error"] = "Invalid employee data.";
				return RedirectToAction(nameof(Index));
			}

			employees.EmployeeId = Guid.NewGuid();
			if (employees.ExpertiseAreaId != null)
			{
				employees.ExpertiseArea = await _context.operations
					.FirstOrDefaultAsync(o => o.OperationId == employees.ExpertiseAreaId);
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

			var employees = await _context.employees.FindAsync(id);
			if (employees == null)
			{
				TempData["Error"] = "Employee not found.";
				return RedirectToAction(nameof(Index));
			}

			ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
			return View(employees);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Employees employees)
		{
			if (!ModelState.IsValid)
			{
				ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
				return View(employees);
			}

			if (employees == null)
			{
				TempData["Error"] = "Invalid employee data.";
				return RedirectToAction(nameof(Index));
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
					TempData["Error"] = "Employee does not exist.";
					return RedirectToAction(nameof(Index));
				}
				else
				{
					throw;
				}
			}
			catch (Exception ex)
			{
				TempData["Error"] = $"An error occurred: {ex.Message}";
				ViewData["ExpertiseArea"] = new SelectList(_context.operations, "OperationId", "OperationName", employees.ExpertiseAreaId);
				return View(employees);
			}
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
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			if (_context.employees == null)
			{
				TempData["Error"] = "Employee entity is null.";
				return RedirectToAction(nameof(Index));
			}

			var employee = await _context.employees.FindAsync(id);
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
