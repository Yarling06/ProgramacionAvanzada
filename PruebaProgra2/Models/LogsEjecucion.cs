using System;
using System.Collections.Generic;

namespace PruebaProgra2.Models;

public partial class LogsEjecucion
{
    public int LogId { get; set; }

    public int TareaId { get; set; }

    public int EstadoId { get; set; }

    public string? Mensaje { get; set; }

    public DateTime? FechaLog { get; set; }

    public string? Descripcion { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public virtual EstadosTarea Estado { get; set; } = null!;

    public virtual Tarea Tarea { get; set; } = null!;
}
