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
        public ConfiguracionTablaFacturas()
        {
            ToTable("Facturas");

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
                .HasMaxLength(500);

            Property(m => m.VendedorNumeroIdentificacionFiscal)
                .HasMaxLength(50);

            Property(m => m.VendedorNombreOEmpresa)
            .HasMaxLength(50);

            Property(m => m.VendedorDireccion)
            .HasMaxLength(50);

            Property(m => m.VendedorLocalidad)
            .HasMaxLength(50);

            Property(m => m.VendedorProvincia)
            .HasMaxLength(50);

            Property(m => m.VendedorCodigoPostal)
            .HasMaxLength(10);
            
            Property(m => m.CompradorNumeroIdentificacionFiscal)
            .HasMaxLength(50);

            Property(m => m.CompradorNombreOEmpresa)
            .HasMaxLength(50);

            Property(m => m.CompradorDireccion)
            .HasMaxLength(50);

            Property(m => m.CompradorLocalidad)
            .HasMaxLength(50);

            Property(m => m.CompradorProvincia)
            .HasMaxLength(50);

            Property(m => m.CompradorCodigoPostal)
           .HasMaxLength(10);                                
                       
        }
    }

    public class ConfiguracionTablaLineasFacturas : EntityTypeConfiguration<LineaFactura>
    {
        public ConfiguracionTablaLineasFacturas()
        {
            ToTable("FacturasLineas");

            HasRequired(m => m.Factura)
              .WithMany(m => m.Lineas)
              .HasForeignKey(m => m.IdFactura)
              .WillCascadeOnDelete(false);


            Property(m => m.Descripcion)
                     .HasMaxLength(250);        
          
          
        }
    }

}
