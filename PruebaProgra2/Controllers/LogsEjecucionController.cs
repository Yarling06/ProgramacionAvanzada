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
    public class LogsEjecucionController : Controller
    {
        private readonly ProyectoPrograDbContext _context;

        public LogsEjecucionController(ProyectoPrograDbContext context)
        {
            _context = context;
        }

        // GET: LogsEjecucion
        public async Task<IActionResult> Index()
        {
            var proyectoPrograDbContext = _context.LogsEjecucions.Include(l => l.Estado).Include(l => l.Tarea);
            return View(await proyectoPrograDbContext.ToListAsync());
        }

        // GET: LogsEjecucion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logsEjecucion = await _context.LogsEjecucions
                .Include(l => l.Estado)
                .Include(l => l.Tarea)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (logsEjecucion == null)
            {
                return NotFound();
            }

            return View(logsEjecucion);
        }

        // GET: LogsEjecucion/Create
        public IActionResult Create()
        {
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId");
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId");
            return View();
        }

        // POST: LogsEjecucion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LogId,TareaId,EstadoId,Mensaje,FechaLog,Descripcion,FechaCreacion")] LogsEjecucion logsEjecucion)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(logsEjecucion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", logsEjecucion.EstadoId);
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", logsEjecucion.TareaId);
            return View(logsEjecucion);
        }

        // GET: LogsEjecucion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logsEjecucion = await _context.LogsEjecucions.FindAsync(id);
            if (logsEjecucion == null)
            {
                return NotFound();
            }
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", logsEjecucion.EstadoId);
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", logsEjecucion.TareaId);
            return View(logsEjecucion);
        }

        // POST: LogsEjecucion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LogId,TareaId,EstadoId,Mensaje,FechaLog,Descripcion,FechaCreacion")] LogsEjecucion logsEjecucion)
        {
            if (id != logsEjecucion.LogId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(logsEjecucion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LogsEjecucionExists(logsEjecucion.LogId))
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
            ViewData["EstadoId"] = new SelectList(_context.EstadosTareas, "EstadoId", "EstadoId", logsEjecucion.EstadoId);
            ViewData["TareaId"] = new SelectList(_context.Tareas, "TareaId", "TareaId", logsEjecucion.TareaId);
            return View(logsEjecucion);
        }

        // GET: LogsEjecucion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var logsEjecucion = await _context.LogsEjecucions
                .Include(l => l.Estado)
                .Include(l => l.Tarea)
                .FirstOrDefaultAsync(m => m.LogId == id);
            if (logsEjecucion == null)
            {
                return NotFound();
            }

            return View(logsEjecucion);
        }

        // POST: LogsEjecucion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var logsEjecucion = await _context.LogsEjecucions.FindAsync(id);
            if (logsEjecucion != null)
            {
                _context.LogsEjecucions.Remove(logsEjecucion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LogsEjecucionExists(int id)
        {
            return _context.LogsEjecucions.Any(e => e.LogId == id);
        }
    }
}
