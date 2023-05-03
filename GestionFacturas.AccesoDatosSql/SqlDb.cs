using GestionFacturas.Dominio;
using GestionFacturas.Dominio.Clientes;
using Microsoft.EntityFrameworkCore;

namespace GestionFacturas.AccesoDatosSql
{
    public partial class SqlDb : DbContext
    {
        private readonly string _connectionString;

        public const string Esquema = "GestionFacturas";

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlDb).Assembly);
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
