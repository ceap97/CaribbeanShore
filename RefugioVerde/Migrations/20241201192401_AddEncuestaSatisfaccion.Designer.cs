﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RefugioVerde.Models;

#nullable disable

namespace RefugioVerde.Migrations
{
    [DbContext(typeof(RefugioVerdeContext))]
    [Migration("20241201192401_AddEncuestaSatisfaccion")]
    partial class AddEncuestaSatisfaccion
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("RefugioVerde.Models.Cliente", b =>
                {
                    b.Property<int>("ClienteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ClienteId"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DocumentoIdentidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("MunicipioId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ClienteId")
                        .HasName("PK__Cliente__71ABD0875D9F5CC3");

                    b.HasIndex("MunicipioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("Cliente", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Comodidad", b =>
                {
                    b.Property<int>("ComodidadId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ComodidadId"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("ComodidadId")
                        .HasName("PK__Comodida__17756631434E0B31");

                    b.ToTable("Comodidad", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Departamento", b =>
                {
                    b.Property<int>("DepartamentoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartamentoId"));

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("DepartamentoId")
                        .HasName("PK__Departam__66BB0E3E26F02BCA");

                    b.ToTable("Departamento", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Empleado", b =>
                {
                    b.Property<int>("EmpleadoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmpleadoId"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DocumentoIdentidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("MunicipioId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("EmpleadoId")
                        .HasName("PK__Empleado__958BE910D4249792");

                    b.HasIndex("MunicipioId");

                    b.HasIndex("RolId");

                    b.ToTable("Empleado", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.EncuestaSatisfaccion", b =>
                {
                    b.Property<int>("EncuestaSatisfaccionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EncuestaSatisfaccionId"));

                    b.Property<int>("CalificacionAtencion")
                        .HasColumnType("int");

                    b.Property<int>("CalificacionGeneral")
                        .HasColumnType("int");

                    b.Property<int>("CalificacionHabitacion")
                        .HasColumnType("int");

                    b.Property<string>("Comentarios")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("EncuestaSatisfaccionId");

                    b.ToTable("EncuestasSatisfaccion");
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoHabitacion", b =>
                {
                    b.Property<int>("EstadoHabitacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstadoHabitacionId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EstadoHabitacionId")
                        .HasName("PK__EstadoHa__43B2B0C382D1B165");

                    b.ToTable("EstadoHabitacion", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoPago", b =>
                {
                    b.Property<int>("EstadoPagoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstadoPagoId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EstadoPagoId")
                        .HasName("PK__EstadoPa__63AD309DF3250763");

                    b.ToTable("EstadoPago", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoReserva", b =>
                {
                    b.Property<int>("EstadoReservaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EstadoReservaId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("EstadoReservaId")
                        .HasName("PK__EstadoRe__DB6E9F21C72345A2");

                    b.ToTable("EstadoReserva", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Habitacion", b =>
                {
                    b.Property<int>("HabitacionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HabitacionId"));

                    b.Property<int>("EstadoHabitacionId")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreHabitacion")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Numero")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("Tipo")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("HabitacionId")
                        .HasName("PK__Habitaci__11AD446177054E9C");

                    b.HasIndex("EstadoHabitacionId");

                    b.ToTable("Habitacion", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Huesped", b =>
                {
                    b.Property<int>("HuespedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("HuespedId"));

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("DocumentoIdentidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("MunicipioId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("ReservaId")
                        .HasColumnType("int");

                    b.Property<string>("Telefono")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("HuespedId")
                        .HasName("PK__Huesped__84BAE2FE3D58B547");

                    b.HasIndex("MunicipioId");

                    b.HasIndex("ReservaId");

                    b.ToTable("Huesped", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Municipio", b =>
                {
                    b.Property<int>("MunicipioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MunicipioId"));

                    b.Property<int>("DepartamentoId")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("MunicipioId")
                        .HasName("PK__Municipi__1EEFE54EB56F0B47");

                    b.HasIndex("DepartamentoId");

                    b.ToTable("Municipio", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Pago", b =>
                {
                    b.Property<int>("IdPago")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdPago"));

                    b.Property<string>("Comprobante")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EstadoPagoId")
                        .HasColumnType("int");

                    b.Property<string>("MetodoPago")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Monto")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int>("ReservaId")
                        .HasColumnType("int");

                    b.HasKey("IdPago")
                        .HasName("PK__Pago__FC851A3A58AE8A76");

                    b.HasIndex("EstadoPagoId");

                    b.HasIndex("ReservaId");

                    b.ToTable("Pago", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Permiso", b =>
                {
                    b.Property<int>("PermisoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PermisoId"));

                    b.Property<string>("Descripcion")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("PermisoId")
                        .HasName("PK__Permiso__96E0C72360593694");

                    b.ToTable("Permiso", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Reserva", b =>
                {
                    b.Property<int>("ReservaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReservaId"));

                    b.Property<int?>("ClienteId")
                        .HasColumnType("int");

                    b.Property<int?>("ComodidadId")
                        .HasColumnType("int");

                    b.Property<int?>("EstadoReservaId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaFin")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("FechaInicio")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("FechaReserva")
                        .HasColumnType("datetime");

                    b.Property<int?>("HabitacionId")
                        .HasColumnType("int");

                    b.Property<int?>("ServicioId")
                        .HasColumnType("int");

                    b.HasKey("ReservaId")
                        .HasName("PK__Reserva__C39937639457F156");

                    b.HasIndex("ClienteId");

                    b.HasIndex("ComodidadId");

                    b.HasIndex("EstadoReservaId");

                    b.HasIndex("HabitacionId");

                    b.HasIndex("ServicioId");

                    b.ToTable("Reserva", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Role", b =>
                {
                    b.Property<int>("RolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RolId"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("RolId")
                        .HasName("PK__Roles__F92302F11BC8EF8E");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("RefugioVerde.Models.Servicio", b =>
                {
                    b.Property<int>("ServicioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ServicioId"));

                    b.Property<string>("Descripcion")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<byte[]>("Imagen")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Precio")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("ServicioId")
                        .HasName("PK__Servicio__D5AEECC2DD3FE95C");

                    b.ToTable("Servicio", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Usuario", b =>
                {
                    b.Property<int>("UsuarioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UsuarioId"));

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("EmpleadoId")
                        .HasColumnType("int");

                    b.Property<string>("Imagen")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("UsuarioId")
                        .HasName("PK__Usuario__2B3DE7B8EDD7E5B8");

                    b.HasIndex("EmpleadoId");

                    b.ToTable("Usuario", (string)null);
                });

            modelBuilder.Entity("RolPermiso", b =>
                {
                    b.Property<int>("RolId")
                        .HasColumnType("int");

                    b.Property<int>("PermisoId")
                        .HasColumnType("int");

                    b.HasKey("RolId", "PermisoId")
                        .HasName("PK__RolPermi__D04D0E83D001992B");

                    b.HasIndex("PermisoId");

                    b.ToTable("RolPermiso", (string)null);
                });

            modelBuilder.Entity("RefugioVerde.Models.Cliente", b =>
                {
                    b.HasOne("RefugioVerde.Models.Municipio", "Municipio")
                        .WithMany("Clientes")
                        .HasForeignKey("MunicipioId")
                        .HasConstraintName("FK__Cliente__Municip__6FE99F9F");

                    b.HasOne("RefugioVerde.Models.Usuario", "Usuario")
                        .WithMany("Clientes")
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Cliente__Usuario__6EF57B66");

                    b.Navigation("Municipio");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("RefugioVerde.Models.Empleado", b =>
                {
                    b.HasOne("RefugioVerde.Models.Municipio", "Municipio")
                        .WithMany("Empleados")
                        .HasForeignKey("MunicipioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Empleado__Munici__68487DD7");

                    b.HasOne("RefugioVerde.Models.Role", "Rol")
                        .WithMany("Empleados")
                        .HasForeignKey("RolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Empleado__RolId__693CA210");

                    b.Navigation("Municipio");

                    b.Navigation("Rol");
                });

            modelBuilder.Entity("RefugioVerde.Models.Habitacion", b =>
                {
                    b.HasOne("RefugioVerde.Models.EstadoHabitacion", "EstadoHabitacion")
                        .WithMany("Habitacions")
                        .HasForeignKey("EstadoHabitacionId")
                        .IsRequired()
                        .HasConstraintName("FK__Habitacio__Estad__5812160E");

                    b.Navigation("EstadoHabitacion");
                });

            modelBuilder.Entity("RefugioVerde.Models.Huesped", b =>
                {
                    b.HasOne("RefugioVerde.Models.Municipio", "Municipio")
                        .WithMany("Huespeds")
                        .HasForeignKey("MunicipioId")
                        .HasConstraintName("FK__Huesped__Municip__7E37BEF6");

                    b.HasOne("RefugioVerde.Models.Reserva", "Reserva")
                        .WithMany("Huespeds")
                        .HasForeignKey("ReservaId")
                        .HasConstraintName("FK__Huesped__Reserva__7D439ABD");

                    b.Navigation("Municipio");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("RefugioVerde.Models.Municipio", b =>
                {
                    b.HasOne("RefugioVerde.Models.Departamento", "Departamento")
                        .WithMany("Municipios")
                        .HasForeignKey("DepartamentoId")
                        .IsRequired()
                        .HasConstraintName("FK__Municipio__Depar__4BAC3F29");

                    b.Navigation("Departamento");
                });

            modelBuilder.Entity("RefugioVerde.Models.Pago", b =>
                {
                    b.HasOne("RefugioVerde.Models.EstadoPago", "EstadoPago")
                        .WithMany("Pagos")
                        .HasForeignKey("EstadoPagoId")
                        .HasConstraintName("FK__Pago__EstadoPago__7A672E12");

                    b.HasOne("RefugioVerde.Models.Reserva", "Reserva")
                        .WithMany("Pagos")
                        .HasForeignKey("ReservaId")
                        .IsRequired()
                        .HasConstraintName("FK__Pago__ReservaId__797309D9");

                    b.Navigation("EstadoPago");

                    b.Navigation("Reserva");
                });

            modelBuilder.Entity("RefugioVerde.Models.Reserva", b =>
                {
                    b.HasOne("RefugioVerde.Models.Cliente", "Cliente")
                        .WithMany("Reservas")
                        .HasForeignKey("ClienteId")
                        .HasConstraintName("FK__Reserva__Cliente__72C60C4A");

                    b.HasOne("RefugioVerde.Models.Comodidad", "Comodidad")
                        .WithMany("Reservas")
                        .HasForeignKey("ComodidadId")
                        .HasConstraintName("FK__Reserva__Comodid__76969D2E");

                    b.HasOne("RefugioVerde.Models.EstadoReserva", "EstadoReserva")
                        .WithMany("Reservas")
                        .HasForeignKey("EstadoReservaId")
                        .HasConstraintName("FK__Reserva__EstadoR__74AE54BC");

                    b.HasOne("RefugioVerde.Models.Habitacion", "Habitacion")
                        .WithMany("Reservas")
                        .HasForeignKey("HabitacionId")
                        .HasConstraintName("FK__Reserva__Habitac__73BA3083");

                    b.HasOne("RefugioVerde.Models.Servicio", "Servicio")
                        .WithMany("Reservas")
                        .HasForeignKey("ServicioId")
                        .HasConstraintName("FK__Reserva__Servici__75A278F5");

                    b.Navigation("Cliente");

                    b.Navigation("Comodidad");

                    b.Navigation("EstadoReserva");

                    b.Navigation("Habitacion");

                    b.Navigation("Servicio");
                });

            modelBuilder.Entity("RefugioVerde.Models.Usuario", b =>
                {
                    b.HasOne("RefugioVerde.Models.Empleado", "Empleado")
                        .WithMany("Usuarios")
                        .HasForeignKey("EmpleadoId")
                        .HasConstraintName("FK__Usuario__Emplead__6C190EBB");

                    b.Navigation("Empleado");
                });

            modelBuilder.Entity("RolPermiso", b =>
                {
                    b.HasOne("RefugioVerde.Models.Permiso", null)
                        .WithMany()
                        .HasForeignKey("PermisoId")
                        .IsRequired()
                        .HasConstraintName("FK__RolPermis__Permi__5FB337D6");

                    b.HasOne("RefugioVerde.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolId")
                        .IsRequired()
                        .HasConstraintName("FK__RolPermis__RolId__5EBF139D");
                });

            modelBuilder.Entity("RefugioVerde.Models.Cliente", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RefugioVerde.Models.Comodidad", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RefugioVerde.Models.Departamento", b =>
                {
                    b.Navigation("Municipios");
                });

            modelBuilder.Entity("RefugioVerde.Models.Empleado", b =>
                {
                    b.Navigation("Usuarios");
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoHabitacion", b =>
                {
                    b.Navigation("Habitacions");
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoPago", b =>
                {
                    b.Navigation("Pagos");
                });

            modelBuilder.Entity("RefugioVerde.Models.EstadoReserva", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RefugioVerde.Models.Habitacion", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RefugioVerde.Models.Municipio", b =>
                {
                    b.Navigation("Clientes");

                    b.Navigation("Empleados");

                    b.Navigation("Huespeds");
                });

            modelBuilder.Entity("RefugioVerde.Models.Reserva", b =>
                {
                    b.Navigation("Huespeds");

                    b.Navigation("Pagos");
                });

            modelBuilder.Entity("RefugioVerde.Models.Role", b =>
                {
                    b.Navigation("Empleados");
                });

            modelBuilder.Entity("RefugioVerde.Models.Servicio", b =>
                {
                    b.Navigation("Reservas");
                });

            modelBuilder.Entity("RefugioVerde.Models.Usuario", b =>
                {
                    b.Navigation("Clientes");
                });
#pragma warning restore 612, 618
        }
    }
}
