using System;
using System.Collections.Generic;

namespace PruebaProgra2.Models;

public partial class EstadosTarea
{
    public int EstadoId { get; set; }

    public string NombreEstado { get; set; } = null!;

    public virtual ICollection<LogsEjecucion> LogsEjecucions { get; set; } = new List<LogsEjecucion>();

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
