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
        public int? IdCliente { get; set; }

        public string NombreOEmpresaCliente { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        public string SerieFactura { get; set; }

        public string Conceptos { get; set; }
        
        public string Comando { get; set; }

        private int _indicePagina;

        public int IndicePagina
        {
            get { return _indicePagina <= 0 ? 1 : _indicePagina; }
            set { _indicePagina = value; }
        }

        public int LineasPorPagina { get { return 25; } }


        public OrdenFacturasEnum OrdenarPorEnum { get; set; }

        public bool TieneValores
        {
            get
            {
                return !string.IsNullOrEmpty(NombreOEmpresaCliente) ||
                    FechaDesde.HasValue || FechaHasta.HasValue ||
                    !string.IsNullOrEmpty(Comando) || 
                    IdCliente.HasValue;
            }
        }
    }
    public enum OrdenFacturasEnum
    {
        [Display(Name = @"Número")]
        NumeroDesc = 0,
        [Display(Name = @"Número (antiguas)")]
        NumeroAsc = 1,
        [Display(Name = @"Fecha")]
        FechaDesc = 2,
        [Display(Name = @"Fecha (antiguas)")]
        FechaAsc = 3,

    }
}
