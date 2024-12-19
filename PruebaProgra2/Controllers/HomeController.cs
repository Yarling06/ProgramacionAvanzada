using Microsoft.AspNetCore.Mvc;
using PruebaProgra2.Models;
using System.Diagnostics;

namespace PruebaProgra2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProyectoPrograDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ProyectoPrograDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HttpGet]
        public JsonResult GetDashboardData()
        {
            var estados = _context.Tareas
                .GroupBy(t => t.Estado)
                .Select(g => new
                {
                    Estado = g.Key.NombreEstado, // Asegúrate de mapear el nombre del estado
                    Total = g.Count()
                })
                .ToList();

            var prioridades = _context.Tareas
                .GroupBy(t => t.Prioridad)
                .Select(g => new
                {
                    Prioridad = g.Key.NivelPrioridad, // Asegúrate de mapear el nivel de prioridad
                    Total = g.Count()
                })
                .ToList();

            return Json(new { estados, prioridades });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
