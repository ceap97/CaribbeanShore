using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using RefugioVerde.Models;

namespace RefugioVerde.Models;

public partial class RefugioVerdeContext : DbContext
{
    public RefugioVerdeContext()
    {
    }

    public RefugioVerdeContext(DbContextOptions<RefugioVerdeContext> options)
        : base(options)
    {
    }
    public DbSet<EncuestaSatisfaccion> EncuestasSatisfaccion { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Comodidad> Comodidads { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<EstadoHabitacion> EstadoHabitacions { get; set; }

    public virtual DbSet<EstadoPago> EstadoPagos { get; set; }

    public virtual DbSet<EstadoReserva> EstadoReservas { get; set; }

    public virtual DbSet<Habitacion> Habitacions { get; set; }

    public virtual DbSet<Huesped> Huespeds { get; set; }
    public virtual DbSet<MetodoDePago> MetodoDePagos { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }


    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("server=ALEJA\\SQLEXPRESS; database=RefugioVerde; integrated security=true; Encrypt=false;Trusted_Connection=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EncuestaSatisfaccion>(entity =>
        {
            entity.HasKey(e => e.EncuestaSatisfaccionId);

            entity.Property(e => e.Comentarios).HasMaxLength(500);

            entity.HasOne(e => e.Habitacion)
                .WithMany(h => h.EncuestasSatisfaccion)
                .HasForeignKey(e => e.HabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EncuestaSatisfaccion_Habitacion");

            entity.HasOne(e => e.Empleado)
                .WithMany(emp => emp.EncuestasSatisfaccion)
                .HasForeignKey(e => e.EmpleadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EncuestaSatisfaccion_Empleado");
        });
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId).HasName("PK__Cliente__71ABD0875D9F5CC3");

            entity.ToTable("Cliente");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__Cliente__Usuario__6EF57B66");
        });

        modelBuilder.Entity<Comodidad>(entity =>
        {
            entity.HasKey(e => e.ComodidadId).HasName("PK__Comodida__17756631434E0B31");

            entity.ToTable("Comodidad");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.DepartamentoId).HasName("PK__Departam__66BB0E3E26F02BCA");

            entity.ToTable("Departamento");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.EmpleadoId).HasName("PK__Empleado__958BE910D4249792");

            entity.ToTable("Empleado");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Rol).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__Empleado__RolId__693CA210");
        });

        modelBuilder.Entity<EstadoHabitacion>(entity =>
        {
            entity.HasKey(e => e.EstadoHabitacionId).HasName("PK__EstadoHa__43B2B0C382D1B165");

            entity.ToTable("EstadoHabitacion");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<EstadoPago>(entity =>
        {
            entity.HasKey(e => e.EstadoPagoId).HasName("PK__EstadoPa__63AD309DF3250763");

            entity.ToTable("EstadoPago");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<EstadoReserva>(entity =>
        {
            entity.HasKey(e => e.EstadoReservaId).HasName("PK__EstadoRe__DB6E9F21C72345A2");

            entity.ToTable("EstadoReserva");

            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Habitacion>(entity =>
        {
            entity.HasKey(e => e.HabitacionId).HasName("PK__Habitaci__11AD446177054E9C");

            entity.ToTable("Habitacion");

            entity.Property(e => e.NombreHabitacion).HasMaxLength(100);
            entity.Property(e => e.Numero).HasMaxLength(10);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Tipo).HasMaxLength(50);

            entity.HasOne(d => d.EstadoHabitacion).WithMany(p => p.Habitacions)
                .HasForeignKey(d => d.EstadoHabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Habitacio__Estad__5812160E");
        });

        modelBuilder.Entity<Huesped>(entity =>
        {
            entity.HasKey(e => e.HuespedId).HasName("PK__Huesped__84BAE2FE3D58B547");

            entity.ToTable("Huesped");

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.DocumentoIdentidad).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Reserva).WithMany(p => p.Huespeds)
                .HasForeignKey(d => d.ReservaId)
                .HasConstraintName("FK__Huesped__Reserva__7D439ABD");
        });

        modelBuilder.Entity<MetodoDePago>(entity =>
        {
            entity.HasKey(e => e.MetodoDePagoId).HasName("PK__MetodoDePago__123456789");

            entity.ToTable("MetodoDePago");

            entity.Property(e => e.Nombre)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasMany(e => e.Pagos)
                .WithOne(p => p.MetodoDePago)
                .HasForeignKey(p => p.MetodoDePagoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pago__MetodoDePago__123456789");
        });
       

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__FC851A3A58AE8A76");

            entity.ToTable("Pago");

            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.MetodoDePago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MetodoDePagoId)
                .HasConstraintName("FK__Pago__MetodoDePa__7B5B524B");
            entity.HasOne(d => d.EstadoPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.EstadoPagoId)
                .HasConstraintName("FK__Pago__EstadoPago__7A672E12");

            entity.HasOne(d => d.Reserva).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.ReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pago__ReservaId__797309D9");
        });


        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.PermisoId).HasName("PK__Permiso__96E0C72360593694");

            entity.ToTable("Permiso");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reserva__C39937639457F156");

            entity.ToTable("Reserva");

            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaReserva).HasColumnType("datetime");

            entity.HasOne(d => d.Cliente).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.ClienteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reserva__Cliente__72C60C4A");

            entity.HasOne(d => d.EstadoReserva).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.EstadoReservaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reserva__EstadoR__74AE54BC");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.HabitacionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reserva__Habitac__73BA3083");

            entity.HasMany(r => r.Comodidades)
                .WithMany(c => c.Reservas)
                .UsingEntity<Dictionary<string, object>>(
                    "ReservaComodidad",
                    r => r.HasOne<Comodidad>().WithMany().HasForeignKey("ComodidadId"),
                    c => c.HasOne<Reserva>().WithMany().HasForeignKey("ReservaId"));

            entity.HasMany(r => r.Servicios)
                .WithMany(s => s.Reservas)
                .UsingEntity<Dictionary<string, object>>(
                    "ReservaServicio",
                    r => r.HasOne<Servicio>().WithMany().HasForeignKey("ServicioId"),
                    s => s.HasOne<Reserva>().WithMany().HasForeignKey("ReservaId"));
        });


        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F11BC8EF8E");

            entity.Property(e => e.Nombre).HasMaxLength(50);

            entity.HasMany(d => d.Permisos).WithMany(p => p.Rols)
                .UsingEntity<Dictionary<string, object>>(
                    "RolPermiso",
                    r => r.HasOne<Permiso>().WithMany()
                        .HasForeignKey("PermisoId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__Permi__5FB337D6"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__RolPermis__RolId__5EBF139D"),
                    j =>
                    {
                        j.HasKey("RolId", "PermisoId").HasName("PK__RolPermi__D04D0E83D001992B");
                        j.ToTable("RolPermiso");
                    });
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEECC2DD3FE95C");

            entity.ToTable("Servicio");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuario__2B3DE7B8EDD7E5B8");

            entity.ToTable("Usuario");

            entity.Property(e => e.Clave).HasMaxLength(255);
            entity.Property(e => e.Correo).HasMaxLength(100);
            entity.Property(e => e.NombreUsuario).HasMaxLength(50);

            entity.HasOne(d => d.Empleado).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.EmpleadoId)
                .HasConstraintName("FK__Usuario__Emplead__6C190EBB");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

//public DbSet<RefugioVerde.Models.MetodoDePago> MetodoDePago { get; set; }
}
