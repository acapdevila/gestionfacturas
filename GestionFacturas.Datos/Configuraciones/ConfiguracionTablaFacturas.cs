using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Datos.Configuraciones
{
    public class ConfiguracionTablaFacturas : EntityTypeConfiguration<Factura>
    {
        public ConfiguracionTablaFacturas(string esquema)
        {
            ToTable("Facturas", esquema);

            HasRequired(m => m.Usuario)
              .WithMany(m=>m.Facturas)
              .HasForeignKey(m=>m.IdUsuario)
              .WillCascadeOnDelete(false);

            Property(m => m.SerieFactura)
                    .HasMaxLength(50)
                    .IsRequired();

            Property(m => m.FormatoNumeroFactura)
                .HasMaxLength(50)
                .IsRequired();

            Property(m => m.FechaEmisionFactura)
              .IsRequired();

            Property(m => m.FormaPagoDetalles)
                .HasMaxLength(50);
                     

            Property(m => m.Comentarios)
                .HasMaxLength(250);

            Property(m => m.ComentariosPie)
                .HasMaxLength(800);

            Property(m => m.ComentarioInterno)
                .HasMaxLength(250);

            Property(m => m.VendedorNumeroIdentificacionFiscal)
                .HasMaxLength(50);

            Property(m => m.VendedorNombreOEmpresa)
            .HasMaxLength(50);

            Property(m => m.VendedorDireccion)
            .HasMaxLength(128);

            Property(m => m.VendedorLocalidad)
            .HasMaxLength(50);

            Property(m => m.VendedorProvincia)
            .HasMaxLength(50);

            Property(m => m.VendedorCodigoPostal)
            .HasMaxLength(10);

            Property(m => m.VendedorEmail)
         .HasMaxLength(50);

            Property(m => m.CompradorNumeroIdentificacionFiscal)
            .HasMaxLength(50);

            Property(m => m.CompradorNombreOEmpresa)
            .HasMaxLength(50);

            Property(m => m.CompradorDireccion)
            .HasMaxLength(128);

            Property(m => m.CompradorLocalidad)
            .HasMaxLength(50);

            Property(m => m.CompradorProvincia)
            .HasMaxLength(50);

            Property(m => m.CompradorCodigoPostal)
           .HasMaxLength(10);

            Property(m => m.CompradorEmail)
         .HasMaxLength(50);
            
            Property(m => m.NombreArchivoPlantillaInforme)
                .HasMaxLength(50);

        }
    }

    public class ConfiguracionTablaLineasFacturas : EntityTypeConfiguration<LineaFactura>
    {
        public ConfiguracionTablaLineasFacturas(string esquema)
        {
            ToTable("FacturasLineas", esquema);

            HasRequired(m => m.Factura)
              .WithMany(m => m.Lineas)
              .HasForeignKey(m => m.IdFactura)
              .WillCascadeOnDelete(false);


            Property(m => m.Descripcion)
                     .HasMaxLength(250);

            //Property(m => m.Cantidad)
            //         .HasPrecision(250);


        }
    }

}
