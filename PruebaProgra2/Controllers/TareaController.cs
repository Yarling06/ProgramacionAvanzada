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
    public class TareaController : Controller
    {
        private readonly ProyectoPrograDbContext _context;

        public TareaController(ProyectoPrograDbContext context)
        {
            _context = context;
        }

        // GET: Tarea
        public async Task<IActionResult> Index()
        {
            var proyectoPrograDbContext = _context.Tareas.Include(t => t.Estado).Include(t => t.Prioridad);
            return View(await proyectoPrograDbContext.ToListAsync());
        }

        // GET: Tarea/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.TareaId == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // GET: Tarea/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId");
            ViewData["PrioridadId"] = new SelectList(_context.PrioridadesTareas, "PrioridadId", "PrioridadId");
            return View();
        }

        // POST: Tarea/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TareaId,Nombre,Descripcion,PrioridadId,EstadoId,FechaCreacion,FechaEjecucion,FechaFinalizacion")] Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(tarea);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", tarea.EstadoId);
            ViewData["PrioridadId"] = new SelectList(_context.PrioridadesTareas, "PrioridadId", "PrioridadId", tarea.PrioridadId);
            return View(tarea);
        }

        // GET: Tarea/Edit/5
        [HttpGet("Tarea/Edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", tarea.EstadoId);
            ViewData["PrioridadId"] = new SelectList(_context.PrioridadesTareas, "PrioridadId", "PrioridadId", tarea.PrioridadId);
            return View(tarea);
        }

        // POST: Tarea/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Tarea/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TareaId,Nombre,Descripcion,PrioridadId,EstadoId,FechaCreacion,FechaEjecucion,FechaFinalizacion")] Tarea tarea)
        {
            if (id != tarea.TareaId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(tarea);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TareaExists(tarea.TareaId))
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
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", tarea.EstadoId);
            ViewData["PrioridadId"] = new SelectList(_context.PrioridadesTareas, "PrioridadId", "PrioridadId", tarea.PrioridadId);
            return View(tarea);
        }

        // GET: Tarea/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tarea = await _context.Tareas
                .Include(t => t.Estado)
                .Include(t => t.Prioridad)
                .FirstOrDefaultAsync(m => m.TareaId == id);
            if (tarea == null)
            {
                return NotFound();
            }

            return View(tarea);
        }

        // POST: Tarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.TareaId == id);
        }
    }
}
