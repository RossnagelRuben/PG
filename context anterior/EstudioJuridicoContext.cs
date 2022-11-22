using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sistema_De_Gestion.Models
{
    public partial class EstudioJuridicoContext : DbContext
    {
        public EstudioJuridicoContext()
        {
        }

        public EstudioJuridicoContext(DbContextOptions<EstudioJuridicoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TableAccione> TableAcciones { get; set; }
        public virtual DbSet<TableCliente> TableClientes { get; set; }
        public virtual DbSet<TableCorreo> TableCorreos { get; set; }
        public virtual DbSet<TablePermiso> TablePermisos { get; set; }
        public virtual DbSet<TableRelacionClienteTramite> TableRelacionClienteTramites { get; set; }
        public virtual DbSet<TableRole> TableRoles { get; set; }
        public virtual DbSet<TableSubTramite> TableSubTramites { get; set; }
        public virtual DbSet<TableTelefono> TableTelefonos { get; set; }
        public virtual DbSet<TableTipoTramite> TableTipoTramites { get; set; }
        public virtual DbSet<TableUsuario> TableUsuarios { get; set; }
        public virtual DbSet<TableVentana> TableVentanas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("server=UNSAFECODE\\SQL; database=EstudioJuridico; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TableAccione>(entity =>
            {
                entity.HasKey(e => e.IdAccion);

                entity.ToTable("Table_Acciones");

                entity.Property(e => e.IdAccion).HasColumnName("ID_Accion");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.IdVentana).HasColumnName("ID_Ventana");

                entity.HasOne(d => d.IdVentanaNavigation)
                    .WithMany(p => p.TableAcciones)
                    .HasForeignKey(d => d.IdVentana)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Acciones_Table_Ventanas");
            });

            modelBuilder.Entity<TableCliente>(entity =>
            {
                entity.HasKey(e => e.IdCliente);

                entity.ToTable("Table_Clientes");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.Apellidos)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Direccion)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Dni).HasColumnName("DNI");

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta");

                entity.Property(e => e.FechaUltModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Ult_Modificacion");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("LINK");

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Obs)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.PassAfip)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Pass_Afip");

                entity.Property(e => e.PassAnses)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("Pass_Anses");
            });

            modelBuilder.Entity<TableCorreo>(entity =>
            {
                entity.HasKey(e => e.IdCorreo);

                entity.ToTable("Table_Correos");

                entity.Property(e => e.IdCorreo).HasColumnName("ID_Correo");

                entity.Property(e => e.Correo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta");

                entity.Property(e => e.FechaUltModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Ult_Modificacion");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.Obs)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.TableCorreos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Correos_Table_Clientes");
            });

            modelBuilder.Entity<TablePermiso>(entity =>
            {
                entity.HasKey(e => e.IdPermiso);

                entity.ToTable("Table_Permisos");

                entity.Property(e => e.IdPermiso).HasColumnName("ID_Permiso");

                entity.Property(e => e.IdAccion).HasColumnName("ID_Accion");

                entity.Property(e => e.IdRol).HasColumnName("ID_Rol");

                entity.HasOne(d => d.IdAccionNavigation)
                    .WithMany(p => p.TablePermisos)
                    .HasForeignKey(d => d.IdAccion)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Permisos_Table_Acciones");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.TablePermisos)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Permisos_Table_Roles");
            });

            modelBuilder.Entity<TableRelacionClienteTramite>(entity =>
            {
                entity.HasKey(e => e.Idrelacion);

                entity.ToTable("Table_Relacion_Cliente_Tramites");

                entity.Property(e => e.Idrelacion).HasColumnName("IDRelacion");

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(1500)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta");

                entity.Property(e => e.FechaUltModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Ult_Modificacion");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.IdsubTramite).HasColumnName("IDSubTramite");

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.TableRelacionClienteTramites)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Relacion_Cliente_Tramites_Table_Clientes");

                entity.HasOne(d => d.IdsubTramiteNavigation)
                    .WithMany(p => p.TableRelacionClienteTramites)
                    .HasForeignKey(d => d.IdsubTramite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Relacion_Cliente_Tramites_Table_Sub_Tramites");
            });

            modelBuilder.Entity<TableRole>(entity =>
            {
                entity.HasKey(e => e.IdRol);

                entity.ToTable("Table_Roles");

                entity.Property(e => e.IdRol).HasColumnName("ID_Rol");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

                entity.HasOne(d => d.IdUsuarioNavigation)
                    .WithMany(p => p.TableRoles)
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Table_Roles_Table_Usuarios");
            });

            modelBuilder.Entity<TableSubTramite>(entity =>
            {
                entity.HasKey(e => e.IdsubTramite)
                    .HasName("PK_Table_Sub_Tramites");

                entity.ToTable("Table_Sub_Tramite");

                entity.Property(e => e.IdsubTramite).HasColumnName("IDSubTramite");

                entity.Property(e => e.DescripcionSubTramite)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta");

                entity.Property(e => e.FechaUltModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Ult_Modificacion");

                entity.Property(e => e.Idtramite).HasColumnName("IDTramite");

                entity.Property(e => e.TituloSubTramite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdtramiteNavigation)
                    .WithMany(p => p.TableSubTramites)
                    .HasForeignKey(d => d.Idtramite)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Sub_Tramites_Table_Tipo_Tramites");
            });

            modelBuilder.Entity<TableTelefono>(entity =>
            {
                entity.HasKey(e => e.IdTelefono);

                entity.ToTable("Table_Telefonos");

                entity.Property(e => e.IdTelefono).HasColumnName("ID_Telefono");

                entity.Property(e => e.FechaAlta).HasColumnType("datetime");

                entity.Property(e => e.FechaUltModificacion).HasColumnType("datetime");

                entity.Property(e => e.IdCliente).HasColumnName("ID_Cliente");

                entity.Property(e => e.Obs)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdClienteNavigation)
                    .WithMany(p => p.TableTelefonos)
                    .HasForeignKey(d => d.IdCliente)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Telefonos_Table_Clientes");
            });

            modelBuilder.Entity<TableTipoTramite>(entity =>
            {
                entity.HasKey(e => e.Idtramite)
                    .HasName("PK_Table_Tipo_Tramites");

                entity.ToTable("Table_Tipo_Tramite");

                entity.Property(e => e.Idtramite).HasColumnName("IDTramite");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FechaAlta)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Alta");

                entity.Property(e => e.FechaUltModificacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Fecha_Ult_Modificacion");

                entity.Property(e => e.Organismo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.TituloTramite)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<TableUsuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("Table_Usuarios");

                entity.Property(e => e.IdUsuario).HasColumnName("ID_Usuario");

                entity.Property(e => e.IdCorreo).HasColumnName("ID_Correo");

                entity.Property(e => e.IdRol).HasColumnName("ID_Rol");

                entity.Property(e => e.IdTelefono).HasColumnName("ID_Telefono");

                entity.Property(e => e.Pass).IsRequired();

                entity.Property(e => e.Token).IsRequired();

                entity.Property(e => e.Usuario)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdCorreoNavigation)
                    .WithMany(p => p.TableUsuarios)
                    .HasForeignKey(d => d.IdCorreo)
                    .HasConstraintName("FK_Table_Usuarios_Table_Correos");

                entity.HasOne(d => d.IdRolNavigation)
                    .WithMany(p => p.TableUsuarios)
                    .HasForeignKey(d => d.IdRol)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Table_Usuarios_Table_Roles");

                entity.HasOne(d => d.IdTelefonoNavigation)
                    .WithMany(p => p.TableUsuarios)
                    .HasForeignKey(d => d.IdTelefono)
                    .HasConstraintName("FK_Table_Usuarios_Table_Telefonos");
            });

            modelBuilder.Entity<TableVentana>(entity =>
            {
                entity.HasKey(e => e.IdVentana);

                entity.ToTable("Table_Ventanas");

                entity.Property(e => e.IdVentana).HasColumnName("ID_Ventana");

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
