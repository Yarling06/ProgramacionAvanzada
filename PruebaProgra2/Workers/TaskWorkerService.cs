using Microsoft.Extensions.DependencyInjection;
using PruebaProgra2.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PruebaProgra2.Workers
{
    public class TaskWorkerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private Task _executingTask;
        public static List<string> WorkerErrors { get; private set; } = new List<string>();


        // Esto es para darle un tiempo de vida al Worker, ya que si no el worker trabajaría de Inicio a Fin de la aplicación y eso puede causar problemas
        public TaskWorkerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // Esto es lo que usamos para que el Worker Inicie
        public async Task StartWorker()
        {
            if (_executingTask != null && !_executingTask.IsCompleted)
            {
                Console.WriteLine("El worker ya está en ejecución.");
                return;
            }

            Console.WriteLine("Iniciando el worker...");
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _executingTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var _dbContext = scope.ServiceProvider.GetRequiredService<ProyectoPrograDbContext>();

                        Console.WriteLine("Worker ejecutándose...");

                        var tareaPendiente = _dbContext.Tareas
                            .Where(t => t.EstadoId == 1)
                            .OrderBy(t => t.PrioridadId)
                            .ThenBy(t => t.FechaEjecucion)
                            .FirstOrDefault();

                        if (tareaPendiente != null)
                        {
                            tareaPendiente.EstadoId = 2;
                            _dbContext.SaveChanges();

                            await Task.Delay(10000, token);

                            var random = new Random();
                            bool falla = random.Next(0, 2) == 0;

                            if (falla)
                            {
                                tareaPendiente.EstadoId = 4;

                                var log = new LogsEjecucion
                                {
                                    TareaId = tareaPendiente.TareaId,
                                    EstadoId = 4,
                                    Mensaje = $"La tarea '{tareaPendiente.Nombre}' falló.",
                                    FechaLog = DateTime.Now
                                };
                                _dbContext.LogsEjecucions.Add(log);

                                WorkerErrors.Add($"La tarea '{tareaPendiente.Nombre}' falló a las {DateTime.Now:HH:mm:ss}.");
                                Console.WriteLine($"Error: La tarea '{tareaPendiente.Nombre}' falló.");
                            }
                            else
                            {
                                tareaPendiente.EstadoId = 3;
                                tareaPendiente.FechaFinalizacion = DateTime.Now;

                                var log = new LogsEjecucion
                                {
                                    TareaId = tareaPendiente.TareaId,
                                    EstadoId = 3,
                                    Mensaje = $"La tarea '{tareaPendiente.Nombre}' se completó exitosamente.",
                                    FechaLog = DateTime.Now
                                };
                                _dbContext.LogsEjecucions.Add(log);

                                Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' completada.");
                            }

                            _dbContext.SaveChanges();
                        }
                        else
                        {
                            Console.WriteLine("No hay tareas pendientes, esperando...");
                            await Task.Delay(10000, token);
                        }
                    }
                }
            }, token);

            Console.WriteLine("Worker iniciado correctamente.");
        }

        public async Task StopWorker()
        {
            if (_executingTask == null || _executingTask.IsCompleted)
            {
                Console.WriteLine("El worker ya está detenido.");
                return;
            }

            Console.WriteLine("Deteniendo el worker...");
            _cancellationTokenSource?.Cancel();

            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token));
            Console.WriteLine("Worker detenido correctamente.");
        }


        // Esto es para detener el worker
    }
}
