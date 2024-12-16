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
    public class OperationsController : Controller
    {
        private readonly MyDbContext _context;

        public OperationsController(MyDbContext context)
        {
            _context = context;
        }


        
        public async Task<IActionResult> Index()
        {
              return _context.operations != null ? 
                          View(await _context.operations.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.operations'  is null.");
        }

        
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.operations == null)
            {
                return NotFound();
            }

            var operations = await _context.operations
                .FirstOrDefaultAsync(m => m.OperationId == id);
            if (operations == null)
            {
                return NotFound();
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
        public async Task<IActionResult> Create([Bind("OperationId,OperationName,Duration,Price")] Operations operations)
        {
            if (operations!=null)
            {
                operations.OperationId = Guid.NewGuid();
                _context.Add(operations);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(operations);
        }

        [HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.operations == null)
            {
                return NotFound();
            }

            var operation = await _context.operations.FirstOrDefaultAsync(o => o.OperationId == id);
            if (operation == null)
            {
                return NotFound();
            }
            return View(operation);
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Operations operations)
        {
            if (operations == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(operations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OperationsExists(operations.OperationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(operations);
        }
        [HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult Delete(Guid id)
        {
            var operation = _context.operations.Find(id);
            if (operation == null)
            {
                return NotFound();
            }
            return View(operation);
        }

        [HttpPost]
		[Authorize(Roles = "Admin")]
		[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Operations operation)
        {
            if (operation == null)
            {
                return NotFound();
            }

            _context.operations.Remove(operation);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }

        private bool OperationsExists(Guid id)
        {
          return (_context.operations?.Any(e => e.OperationId == id)).GetValueOrDefault();
        }
    }
}
