using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    [Serializable]
    public  class FiltroBusquedaFactura
    {   
        public string NombreOEmpresaCliente { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public string NombreArchivoLogo { get; set; }

        public string Comando { get; set; }

        public bool TieneValores
        {
            get
            {
                return !string.IsNullOrEmpty(NombreOEmpresaCliente) ||
                    FechaDesde.HasValue || FechaHasta.HasValue ||
                    !string.IsNullOrEmpty(NombreArchivoLogo) ||
                    !string.IsNullOrEmpty(Comando);
            }
        }
    }
}
