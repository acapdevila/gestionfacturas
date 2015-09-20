using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    [Serializable]
    public  class FiltroBusquedaCliente
    {

        public int? Id { get; set; }
        public string NombreOEmpresa { get; set; }

        public string IdentificacionFiscal { get; set; }

        public string Comando { get; set; }


        public bool TieneValores
        {
            get
            {
                return  !string.IsNullOrEmpty(NombreOEmpresa) || 
                        Id.HasValue || 
                        !string.IsNullOrEmpty(IdentificacionFiscal) || 
                        !string.IsNullOrEmpty(Comando);
            }
        }
    }
}
