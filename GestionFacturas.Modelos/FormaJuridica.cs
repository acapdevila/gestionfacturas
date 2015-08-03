using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    public class FormaJuridica
    {
        public string NumeroIdentificacionFiscal { get; set; }
        public string NombreOEmpresa { get; set; }    
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string CodigoPostal { get; set; }
    }

    public class PersonaFisica : FormaJuridica
    {
        public string Apellidos { get; set; }
        public string Nombre { get { return string.Format("{0} {1}", NombreOEmpresa, Apellidos).Trim(); } }
    }

    public class PersonaJuridica : FormaJuridica
    {
        public string NombreComercial { get; set; }
        public string Nombre { get { return (NombreComercial ?? NombreOEmpresa).Trim(); } }
    }

}
