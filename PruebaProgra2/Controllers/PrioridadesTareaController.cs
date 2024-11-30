using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaProgra2.Models;

namespace PruebaProgra2.Controllers
{
    public class PrioridadesTareaController : Controller
    {
        private readonly ProyectoPrograDbContext _context;

        public PrioridadesTareaController(ProyectoPrograDbContext context)
        {
            _context = context;
        }

        // GET: PrioridadesTarea
        public async Task<IActionResult> Index()
        {
            return View(await _context.PrioridadesTareas.ToListAsync());
        }

        // GET: PrioridadesTarea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prioridadesTarea = await _context.PrioridadesTareas
                .FirstOrDefaultAsync(m => m.PrioridadId == id);
            if (prioridadesTarea == null)
            {
                return NotFound();
            }

            return View(prioridadesTarea);
        }

        // GET: PrioridadesTarea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PrioridadesTarea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PrioridadId,NivelPrioridad")] PrioridadesTarea prioridadesTarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prioridadesTarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(prioridadesTarea);
        }

        // GET: PrioridadesTarea/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prioridadesTarea = await _context.PrioridadesTareas.FindAsync(id);
            if (prioridadesTarea == null)
            {
                return NotFound();
            }
            return View(prioridadesTarea);
        }

        // POST: PrioridadesTarea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PrioridadId,NivelPrioridad")] PrioridadesTarea prioridadesTarea)
        {
            if (id != prioridadesTarea.PrioridadId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prioridadesTarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrioridadesTareaExists(prioridadesTarea.PrioridadId))
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
            return View(prioridadesTarea);
        }

        // GET: PrioridadesTarea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prioridadesTarea = await _context.PrioridadesTareas
                .FirstOrDefaultAsync(m => m.PrioridadId == id);
            if (prioridadesTarea == null)
            {
                return NotFound();
            }

            return View(prioridadesTarea);
        }

        // POST: PrioridadesTarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prioridadesTarea = await _context.PrioridadesTareas.FindAsync(id);
            if (prioridadesTarea != null)
            {
                _context.PrioridadesTareas.Remove(prioridadesTarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrioridadesTareaExists(int id)
        {
            return _context.PrioridadesTareas.Any(e => e.PrioridadId == id);
        }
    }
}
