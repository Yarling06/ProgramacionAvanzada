using System;
using System.Collections.Generic;

namespace PruebaProgra2.Models;

public partial class PrioridadesTarea
{
    public int PrioridadId { get; set; }

    public string NivelPrioridad { get; set; } = null!;

    public virtual ICollection<Tarea> Tareas { get; set; } = new List<Tarea>();
}
