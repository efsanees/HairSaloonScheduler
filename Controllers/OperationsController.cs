using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HairSaloonScheduler.Context;
using HairSaloonScheduler.Models;
using Microsoft.AspNetCore.Authorization;

namespace HairSaloonScheduler.Controllers
{
    public class OperationsController : Controller
    {
        private readonly MyDbContext _context;

        public OperationsController(MyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (_context.operations == null)
            {
                return Problem("Entity set 'MyDbContext.operations' is null.");
            }

            var operations = await _context.operations.ToListAsync();
            if (operations == null || !operations.Any())
            {
                ViewBag.Message = "No operations found.";
                return View("Empty");
            }

            return View(operations);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Operation ID is missing.";
                return RedirectToAction(nameof(Index));
            }

            var operations = await _context.operations.FirstOrDefaultAsync(m => m.OperationId == id);
            if (operations == null)
            {
                TempData["Error"] = "Operation not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(operations);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OperationId,OperationName,Price")] Operations operations, int durationMinutes)
        {
            if (!ModelState.IsValid)
            {
                return View(operations);
            }

            if (operations == null || durationMinutes <= 0)
            {
                TempData["Error"] = "Invalid operation data.";
                return View(operations);
            }

            operations.OperationId = Guid.NewGuid();
            operations.Duration = TimeSpan.FromMinutes(durationMinutes);
            _context.Add(operations);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return View(operations);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Operation ID is missing.";
                return RedirectToAction(nameof(Index));
            }

            var operation = await _context.operations.FirstOrDefaultAsync(o => o.OperationId == id);
            if (operation == null)
            {
                TempData["Error"] = "Operation not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(operation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Operations operations)
        {
            if (!ModelState.IsValid)
            {
                return View(operations);
            }

            if (operations == null)
            {
                TempData["Error"] = "Invalid operation data.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Update(operations);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OperationsExists(operations.OperationId))
                {
                    TempData["Error"] = "Operation does not exist.";
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
                return View(operations);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                TempData["Error"] = "Operation ID is missing.";
                return RedirectToAction(nameof(Index));
            }

            var operation = _context.operations.Find(id);
            if (operation == null)
            {
                TempData["Error"] = "Operation not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(operation);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var operation = await _context.operations.FindAsync(id);
            if (operation == null)
            {
                TempData["Error"] = "Operation not found.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.operations.Remove(operation);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        private bool OperationsExists(Guid id)
        {
            return (_context.operations?.Any(e => e.OperationId == id)).GetValueOrDefault();
        }
    }
}
