using Microsoft.AspNetCore.Mvc;
using PruebaProgra2.Workers;

namespace PruebaProgra2.Controllers
{
    public class WorkerController : Controller
    {
        private readonly TaskWorkerService _taskWorkerService;

        public WorkerController(TaskWorkerService taskWorkerService)
        {
            _taskWorkerService = taskWorkerService;
        }

        // Esto es para iniciar el worker
        [HttpPost]
        public async Task<IActionResult> StartWorker()
        {
            await _taskWorkerService.StartWorker();
            return RedirectToAction("Index", "Tarea"); // Redirige a la vista "Index" de Tarea
        }

        // Esto es para detener el worker
        [HttpPost]
        public async Task<IActionResult> StopWorker()
        {
            await _taskWorkerService.StopWorker();
            return RedirectToAction("Index", "Tarea"); // Redirige a la vista "Index" de Tarea
        }
    }
}