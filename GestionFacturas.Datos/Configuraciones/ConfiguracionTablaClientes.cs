using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Datos.Configuraciones
{
    public class ConfiguracionTablaClientes : EntityTypeConfiguration<Cliente>
    {
        public ConfiguracionTablaClientes(string esquema)
        {

            ToTable("Clientes", esquema);
            
           Property(m => m.ComentarioInterno)
                .HasMaxLength(250);
          
            Property(m => m.NumeroIdentificacionFiscal)
            .HasMaxLength(50);

            Property(m => m.NombreOEmpresa)
            .HasMaxLength(128);

            Property(m => m.NombreComercial)
          .HasMaxLength(50);

            Property(m => m.Direccion)
            .HasMaxLength(128);

            Property(m => m.Localidad)
            .HasMaxLength(50);

            Property(m => m.Provincia)
            .HasMaxLength(50);

            Property(m => m.CodigoPostal)
           .HasMaxLength(10);

            Property(m => m.Email)
                .HasMaxLength(50);

            Property(m => m.PersonaContacto)
                .HasMaxLength(50);

            HasMany(t => t.Facturas)
              .WithOptional(t => t.Comprador)
              .HasForeignKey(m=>m.IdComprador);

        }
    }
    

}
