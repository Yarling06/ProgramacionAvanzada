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
            TempData["WorkerMessage"] = "Worker iniciado correctamente.";
            return RedirectToAction("Index", "Tarea");
        }

        // Esto es para detener el worker
        [HttpPost]
        public async Task<IActionResult> StopWorker()
        {
            await _taskWorkerService.StopWorker();
            TempData["WorkerMessage"] = "Worker detenido correctamente.";
            return RedirectToAction("Index", "Tarea");
        }

        
        public IActionResult ShowWorkerErrors()
        {
            if (TaskWorkerService.WorkerErrors.Any())
            {
                TempData["WorkerErrors"] = string.Join("<br>", TaskWorkerService.WorkerErrors);
                TaskWorkerService.WorkerErrors.Clear(); // Limpia los errores después de mostrarlos
            }
            else
            {
                TempData["WorkerErrors"] = null; // Limpia TempData si no hay errores
            }
            return RedirectToAction("Index", "Tarea");
        }


    }
}