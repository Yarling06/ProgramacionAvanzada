﻿using Microsoft.Extensions.DependencyInjection;
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

        // Esto es para darle un tiempo de vida al Worker, ya que si no el worker trabajaría de Inicio a Fin de la aplicación y eso puede causar problemas
        public TaskWorkerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        // Esto es lo que usamos para que el Worker Inicie
        public async Task StartWorker()
        {
            // Esto es por si el worker ya está corriendo, es para que no haga nada, eso es lo correcto
            if (_executingTask != null && !_executingTask.IsCompleted)
            {
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            // Aquí iniciamos el procesamiento en segundo plano
            _executingTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Accedemos al DbContext
                        var _dbContext = scope.ServiceProvider.GetRequiredService<ProyectoPrograDbContext>();

                        Console.WriteLine("Worker ejecutándose...");

                        // Aquí es donde buscamos las tareas pendientes
                        var tareaPendiente = _dbContext.Tareas
                            .Where(t => t.EstadoId == 1) // 1 es "Pendiente"
                            .OrderBy(t => t.PrioridadId) // Ordena por prioridad
                            .ThenBy(t => t.FechaEjecucion) // Luego por fecha de ejecución
                            .FirstOrDefault();

                        if (tareaPendiente != null)
                        {
                            // Cambiamos el estado de la tarea a "En Proceso"
                            tareaPendiente.EstadoId = 2; // Estado 2 = "En Proceso"
                            _dbContext.SaveChanges();

                            // Simula la ejecución de la tarea
                            await Task.Delay(10000, token);

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
                        }
                        else
                        {
                            // Si no hay tareas pendientes, espera un tiempo más largo antes de buscar de nuevo
                            Console.WriteLine("No hay tareas pendientes, esperando...");
                            await Task.Delay(10000, token);
                        }
                    }
                }
            }, token);
        }

        // Esto es para detener el worker
        public async Task StopWorker()
        {
            // Si el worker no está ejecutándose, no hace nada
            if (_executingTask == null || _executingTask.IsCompleted)
            {
                return;
            }

            // Cancela el token de ejecución
            _cancellationTokenSource?.Cancel();

            // Espera a que el worker finalice de forma ordenada
            await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, _cancellationTokenSource.Token));
        }
    }
}
