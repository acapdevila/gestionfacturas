using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Clientes;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.AccesoDatosSql
{
    public partial class SqlDb : DbContext
    {
        private readonly string _connectionString;

        //public SqlDb()
        //{
        //    _connectionString =
        //        @"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Proyectos\GestionFacturas\GestionFacturas.Datos\BaseDatosLocal\aspnet-GestionFacturas.Website-20150802075020.mdf;Integrated Security=True";

        //}

        public SqlDb(string connectionString) 
        {
            _connectionString = connectionString;
        }

        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Factura> Facturas { get; set; } = null!;
        public virtual DbSet<LineaFactura> FacturasLineas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder
                .UseSqlServer(_connectionString)
                .UseLazyLoadingProxies();
                    
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("AspNetUsers", "GestionFacturas");

                entity.Property(e => e.Email);

                entity.Property(e => e.Password).HasColumnName("PasswordHash");
            });


            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("Clientes", "GestionFacturas");

                entity.Property(e => e.CodigoPostal).HasMaxLength(10);

                entity.Property(e => e.ComentarioInterno)
                    .IsRequired(false)
                    .HasMaxLength(250);

                entity.Property(e => e.Direccion).HasMaxLength(128);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.Localidad).HasMaxLength(50);

                entity.Property(e => e.NombreComercial).HasMaxLength(50);

                entity.Property(e => e.NombreOEmpresa)
                    .HasMaxLength(128)
                    .HasColumnName("NombreOEmpresa");

                entity.Property(e => e.NumeroIdentificacionFiscal).HasMaxLength(50);

                entity.Property(e => e.PersonaContacto).HasMaxLength(50).IsRequired(false);

                entity.Property(e => e.Provincia).HasMaxLength(50);
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.ToTable("Facturas", "GestionFacturas");

                entity.HasIndex(e => e.IdComprador, "IX_IdComprador");

                entity.HasIndex(e => e.IdUsuario, "IX_IdUsuario");

                entity.Property(e => e.ComentarioInterno).HasMaxLength(250).IsRequired(false);

                entity.Property(e => e.Comentarios).HasMaxLength(250).IsRequired(false);

                entity.Property(e => e.ComentariosPie).HasMaxLength(800).IsRequired(false);

                entity.Property(e => e.CompradorCodigoPostal).HasMaxLength(10);

                entity.Property(e => e.CompradorDireccion).HasMaxLength(128);

                entity.Property(e => e.CompradorEmail).HasMaxLength(256);

                entity.Property(e => e.CompradorLocalidad).HasMaxLength(50);

                entity.Property(e => e.CompradorNombreOEmpresa)
                    .HasMaxLength(128)
                    .HasColumnName("CompradorNombreOEmpresa");

                entity.Property(e => e.CompradorNumeroIdentificacionFiscal).HasMaxLength(50);

                entity.Property(e => e.CompradorProvincia).HasMaxLength(50);

                entity.Property(e => e.FechaEmisionFactura).HasColumnType("datetime");

                entity.Property(e => e.FechaVencimientoFactura).HasColumnType("datetime");

                entity.Property(e => e.FormaPagoDetalles).HasMaxLength(50);

                entity.Property(e => e.FormatoNumeroFactura).HasMaxLength(50);

                entity.Property(e => e.IdUsuario).HasMaxLength(128);

                entity.Property(e => e.NombreArchivoPlantillaInforme).HasMaxLength(50);

                entity.Property(e => e.SerieFactura).HasMaxLength(50);

                entity.Property(e => e.VendedorCodigoPostal).HasMaxLength(10);

                entity.Property(e => e.VendedorDireccion).HasMaxLength(128);

                entity.Property(e => e.VendedorEmail).HasMaxLength(256);

                entity.Property(e => e.VendedorLocalidad).HasMaxLength(50);

                entity.Property(e => e.VendedorNombreOEmpresa)
                    .HasMaxLength(50)
                    .HasColumnName("VendedorNombreOEmpresa");

                entity.Property(e => e.VendedorNumeroIdentificacionFiscal).HasMaxLength(50);

                entity.Property(e => e.VendedorProvincia).HasMaxLength(50);

                entity.HasOne(m => m.Usuario)
                    .WithMany()
                    .HasForeignKey(u => u.IdUsuario)
                    .IsRequired();

                entity.HasOne(d => d.Comprador)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.IdComprador)
                    .HasConstraintName("FK_dbo.Facturas_dbo.Clientes_IdComprador");
            });

            modelBuilder.Entity<LineaFactura>(entity =>
            {
                entity.ToTable("FacturasLineas", "GestionFacturas");

                entity.HasIndex(e => e.IdFactura, "IX_IdFactura");

                entity.Property(e => e.Cantidad).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Descripcion).HasMaxLength(250);

                entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Factura)
                    .WithMany(p => p.Lineas)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FacturasLineas_Facturas");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
