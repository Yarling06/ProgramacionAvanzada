using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PruebaProgra2.Models;

public partial class ProyectoPrograDbContext : DbContext
{
    public ProyectoPrograDbContext()
    {
    }

    public ProyectoPrograDbContext(DbContextOptions<ProyectoPrograDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EstadosTarea>? EstadosTareas { get; set; }

    public virtual DbSet<LogsEjecucion>? LogsEjecucions { get; set; }

    public virtual DbSet<PrioridadesTarea>? PrioridadesTareas { get; set; }

    public virtual DbSet<Tarea>? Tareas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-H8F30GDD;Database=ProyectoPrograDB;Trusted_Connection=True;Encrypt=False;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EstadosTarea>(entity =>
        {
            entity.HasKey(e => e.EstadoId).HasName("PK__EstadosT__FEF86B605F61D9F6");

            entity.ToTable("EstadosTarea");

            entity.HasIndex(e => e.NombreEstado, "UQ__EstadosT__6CE50615FBEA5A6A").IsUnique();

            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.NombreEstado).HasMaxLength(50);
        });

        modelBuilder.Entity<LogsEjecucion>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogsEjec__5E5499A88F583209");

            entity.ToTable("LogsEjecucion");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaLog)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TareaId).HasColumnName("TareaID");

            entity.HasOne(d => d.Estado).WithMany(p => p.LogsEjecucions)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogsEjecucion_EstadosTarea");

            entity.HasOne(d => d.Tarea).WithMany(p => p.LogsEjecucions)
                .HasForeignKey(d => d.TareaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogsEjecucion_Tareas");
        });

        modelBuilder.Entity<PrioridadesTarea>(entity =>
        {
            entity.HasKey(e => e.PrioridadId).HasName("PK__Priorida__393917CE6F4E24DE");

            entity.ToTable("PrioridadesTarea");

            entity.HasIndex(e => e.NivelPrioridad, "UQ__Priorida__9A68DDA1AEAA00E4").IsUnique();

            entity.Property(e => e.PrioridadId).HasColumnName("PrioridadID");
            entity.Property(e => e.NivelPrioridad).HasMaxLength(20);
        });

        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.HasKey(e => e.TareaId).HasName("PK__Tareas__5CD836717B6C9285");

            entity.Property(e => e.TareaId).HasColumnName("TareaID");
            entity.Property(e => e.EstadoId).HasColumnName("EstadoID");
            entity.Property(e => e.FechaCreacion)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.FechaEjecucion).HasColumnType("datetime");
            entity.Property(e => e.FechaFinalizacion).HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.PrioridadId).HasColumnName("PrioridadID");

            entity.HasOne(d => d.Estado).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.EstadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tareas_EstadosTarea");

            entity.HasOne(d => d.Prioridad).WithMany(p => p.Tareas)
                .HasForeignKey(d => d.PrioridadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tareas_PrioridadesTarea");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
