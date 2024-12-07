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
    public class OperationsController : Controller
    {
        private readonly MyDbContext _context;

        public OperationsController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Operations
        public async Task<IActionResult> Index()
        {
              return _context.operations != null ? 
                          View(await _context.operations.ToListAsync()) :
                          Problem("Entity set 'MyDbContext.operations'  is null.");
        }

        // GET: Operations/Details/5
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

        // GET: Operations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Operations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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

        // GET: Operations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.operations == null)
            {
                return NotFound();
            }

            var operations = await _context.operations.FindAsync(id);
            if (operations == null)
            {
                return NotFound();
            }
            return View(operations);
        }

        // POST: Operations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("OperationId,OperationName,Duration,Price")] Operations operations)
        {
            if (id != operations.OperationId)
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

        // GET: Operations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Operations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.operations == null)
            {
                return Problem("Entity set 'MyDbContext.operations'  is null.");
            }
            var operations = await _context.operations.FindAsync(id);
            if (operations != null)
            {
                _context.operations.Remove(operations);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OperationsExists(Guid id)
        {
          return (_context.operations?.Any(e => e.OperationId == id)).GetValueOrDefault();
        }
    }
}
