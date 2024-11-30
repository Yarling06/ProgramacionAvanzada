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
    public class EstadosTareaController : Controller
    {
        private readonly ProyectoPrograDbContext _context;

        public EstadosTareaController(ProyectoPrograDbContext context)
        {
            _context = context;
        }

        // GET: EstadosTarea
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosTareas.ToListAsync());
        }

        // GET: EstadosTarea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosTarea = await _context.EstadosTareas
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadosTarea == null)
            {
                return NotFound();
            }

            return View(estadosTarea);
        }

        // GET: EstadosTarea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosTarea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EstadoId,NombreEstado")] EstadosTarea estadosTarea)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estadosTarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estadosTarea);
        }

        // GET: EstadosTarea/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosTarea = await _context.EstadosTareas.FindAsync(id);
            if (estadosTarea == null)
            {
                return NotFound();
            }
            return View(estadosTarea);
        }

        // POST: EstadosTarea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EstadoId,NombreEstado")] EstadosTarea estadosTarea)
        {
            if (id != estadosTarea.EstadoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estadosTarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstadosTareaExists(estadosTarea.EstadoId))
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
            return View(estadosTarea);
        }

        // GET: EstadosTarea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosTarea = await _context.EstadosTareas
                .FirstOrDefaultAsync(m => m.EstadoId == id);
            if (estadosTarea == null)
            {
                return NotFound();
            }

            return View(estadosTarea);
        }

        // POST: EstadosTarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estadosTarea = await _context.EstadosTareas.FindAsync(id);
            if (estadosTarea != null)
            {
                _context.EstadosTareas.Remove(estadosTarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadosTareaExists(int id)
        {
            return _context.EstadosTareas.Any(e => e.EstadoId == id);
        }
    }
}
