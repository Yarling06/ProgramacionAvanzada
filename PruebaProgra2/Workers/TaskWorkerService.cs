using Microsoft.Extensions.DependencyInjection;
using PruebaProgra2.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PruebaProgra2.Services;


namespace PruebaProgra2.Workers
{
    public class TaskWorkerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IEmailSender _emailSender; // Agregar la inyección de dependencia de IEmailSender
        private CancellationTokenSource _cancellationTokenSource;
        private Task _executingTask;

<<<<<<< HEAD
        // Constructor para inicializar el servicio con un Factory de scope
        public TaskWorkerService(IServiceScopeFactory scopeFactory, IEmailSender emailSender)
=======
        // Esto es para darle un tiempo de vida al Worker, ya que si no el worker trabajaría de Inicio a Fin de la aplicación y eso puede causar problemas
        public TaskWorkerService(IServiceScopeFactory scopeFactory)
>>>>>>> 5d4686739fe4947d0d88bac37b7afb130e459583
        {
            _scopeFactory = scopeFactory;
            _emailSender = emailSender; // Asignar el servicio IEmailSender
        }

<<<<<<< HEAD
        // Inicia el Worker en segundo plano
=======
        // Esto es lo que usamos para que el Worker Inicie
>>>>>>> 5d4686739fe4947d0d88bac37b7afb130e459583
        public async Task StartWorker()
        {
            // Si ya hay una tarea ejecutándose, no hacemos nada
            if (_executingTask != null && !_executingTask.IsCompleted)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Inicia el proceso en segundo plano
            _executingTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        try
                        {
                            // Accedemos al DbContext
                            var _dbContext = scope.ServiceProvider.GetRequiredService<ProyectoPrograDbContext>();

                            Console.WriteLine("Worker ejecutándose...");

<<<<<<< HEAD
                            // Buscamos las tareas que están pendientes y cuya fecha de ejecución ha llegado
                            var tareaPendiente = _dbContext.Tareas
                                .Where(t => t.EstadoId == 1) // Estado "Pendiente"
                                .Where(t => t.FechaEjecucion <= DateTime.Now) // Fecha de ejecución pasada o actual
                                .OrderBy(t => t.PrioridadId) // Ordenamos por prioridad
                                .ThenBy(t => t.FechaEjecucion) // Luego por fecha de ejecución
                                .FirstOrDefault();

                            if (tareaPendiente != null)
                            {
                                // Cambiamos el estado de la tarea a "En Proceso"
                                tareaPendiente.EstadoId = 2; // Estado 2 = "En Proceso"
                                tareaPendiente.FechaEjecucion = DateTime.Now; // Establecemos la hora de inicio
                                _dbContext.SaveChanges();

                                Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' iniciada.");

                                // Simula la ejecución de la tarea (reemplazar con código real de ejecución)
                                await Task.Delay(10000, token); // Simula la ejecución durante 10 segundos

                                // Verificamos si la fecha de finalización ha pasado
                                if (tareaPendiente.FechaFinalizacion <= DateTime.Now)
                                {
                                    // Si la fecha de finalización ha pasado, actualizamos el estado a "Finalizado"
                                    tareaPendiente.EstadoId = 3; // Estado 3 = "Finalizado"
                                    tareaPendiente.FechaFinalizacion = DateTime.Now; // Establecemos la fecha de finalización actual
                                    _dbContext.SaveChanges();

                                    Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' finalizada correctamente.");

                                    // Enviar correo de notificación cuando la tarea termine
                                    await _emailSender.SendEmailAsync(
                                        "darinahenry@gmail.com",
                                        "Tarea Finalizada",
                                        $"La tarea '{tareaPendiente.Nombre}' ha finalizado correctamente."
                                    );
                                }
                                else
                                {
                                    // Si la tarea sigue ejecutándose, pero la fecha final no ha llegado, mantenemos el estado en "En Proceso"
                                    Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' aún en ejecución.");
                                }
                            }
                            else
                            {
                                // Si no hay tareas pendientes, esperamos 10 segundos antes de buscar de nuevo
                                Console.WriteLine("No hay tareas pendientes, esperando...");
                                await Task.Delay(10000, token);
                            }

                            // Verificar si ya se ha llegado a la fecha de finalización después de un tiempo (revisar todas las tareas)
                            var tareasPendientes = _dbContext.Tareas
                                .Where(t => t.EstadoId == 2) // Solo las tareas en "En Proceso"
                                .Where(t => t.FechaFinalizacion <= DateTime.Now) // Fecha de finalización pasada o actual
                                .ToList();

                            foreach (var tarea in tareasPendientes)
                            {
                                // Cambiamos el estado de la tarea a "Finalizado"
                                tarea.EstadoId = 3; // Estado 3 = "Finalizado"
                                tarea.FechaFinalizacion = DateTime.Now; // Establecemos la fecha de finalización actual
                                _dbContext.SaveChanges();

                                Console.WriteLine($"Tarea '{tarea.Nombre}' finalizada correctamente.");

                                // Enviar correo de notificación cuando la tarea termine
                                await _emailSender.SendEmailAsync(
                                    "darinahenry@gmail.com",
                                    "Tarea Finalizada",
                                    $"La tarea '{tarea.Nombre}' ha finalizado correctamente."
                                );
                            }
=======
                            // Aquí simulamos si la tarea falla o se completa
                            var random = new Random();
                            bool falla = random.Next(0, 2) == 0; // 50% de probabilidad de fallar

                            if (falla)
                            {
                                // Cambiamos el estado de la tarea a "Error"
                                tareaPendiente.EstadoId = 4; // Estado 4 = "Error"

                                // Registrar en LogsEjecucion
                                var log = new LogsEjecucion
                                {
                                    TareaId = tareaPendiente.TareaId,
                                    EstadoId = 4, // Fallida
                                    Mensaje = $"La tarea '{tareaPendiente.Nombre}' falló.",
                                    FechaLog = DateTime.Now
                                };
                                _dbContext.LogsEjecucions.Add(log);

                                Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' falló.");
                            }
                            else
                            {
                                // Cambiamos el estado de la tarea a "Terminado"
                                tareaPendiente.EstadoId = 3; // Estado 3 = "Terminado"
                                tareaPendiente.FechaFinalizacion = DateTime.Now;

                                // Registrar en LogsEjecucion
                                var log = new LogsEjecucion
                                {
                                    TareaId = tareaPendiente.TareaId,
                                    EstadoId = 3, // Finalizada
                                    Mensaje = $"La tarea '{tareaPendiente.Nombre}' se completó exitosamente.",
                                    FechaLog = DateTime.Now
                                };
                                _dbContext.LogsEjecucions.Add(log);

                                Console.WriteLine($"Tarea '{tareaPendiente.Nombre}' completada.");
                            }

                            _dbContext.SaveChanges();
>>>>>>> 5d4686739fe4947d0d88bac37b7afb130e459583
                        }
                        catch (Exception ex)
                        {
                            // En caso de error, enviamos un correo con los detalles del error
                            Console.WriteLine($"Error: {ex.Message}");

                            await _emailSender.SendEmailAsync(
                                "darinahenry@gmail.com", // Dirección de correo para notificar errores
                                "Error en Worker",
                                $"Se ha producido un error en el Worker: {ex.Message}\n{ex.StackTrace}"
                            );
                        }
                    }
                }
            }, token);
        }

        // Detiene el Worker
        public async Task StopWorker()
        {
            // Si no hay una tarea ejecutándose, no hacemos nada
            if (_executingTask == null || _executingTask.IsCompleted)
            {
                return;
            }

            // Cancela la ejecución
            _cancellationTokenSource?.Cancel();

            // Esperamos a que la tarea finalice correctamente
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token));
        }
    }
}
