using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    
    public class Cliente
    {
        public int Id { get; set; }
        
        public string NumeroIdentificacionFiscal { get; set; }

        public string NombreOEmpresa { get; set; }

        public string Direccion { get; set; }
        
        public string Localidad { get; set; }
        
        public string Provincia { get; set; }
        
        public string CodigoPostal { get; set; }
        
        public string Email { get; set; }              

    }   

}
