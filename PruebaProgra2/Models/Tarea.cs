using System;
using System.Collections.Generic;

namespace PruebaProgra2.Models;

public partial class Tarea
{
    public int TareaId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public int PrioridadId { get; set; }

    public int EstadoId { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaEjecucion { get; set; }

    public DateTime? FechaFinalizacion { get; set; }

    public virtual EstadosTarea Estado { get; set; } = null!;

    public virtual ICollection<LogsEjecucion> LogsEjecucions { get; set; } = new List<LogsEjecucion>();

    public virtual PrioridadesTarea Prioridad { get; set; } = null!;
}
