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


        private int _indicePagina;

        public int IndicePagina
        {
            get { return _indicePagina <= 0 ? 1: _indicePagina; }
            set { _indicePagina = value; }
        }
        

        public int LineasPorPagina { get { return 25; } }


        public OrdenClientesEnum OrdenarPorEnum { get; set; }


        public bool TieneFiltrosBusqueda
        {
            get
            {
                return  !string.IsNullOrEmpty(NombreOEmpresa) || 
                        Id.HasValue || 
                        !string.IsNullOrEmpty(IdentificacionFiscal) || 
                        !string.IsNullOrEmpty(Comando);
            }
        }

        public bool EsPaginacion
        {
            get
            {
                return IndicePagina > 0;
            }
        }       
    }

    public enum OrdenClientesEnum
    {
        [Display(Name = @"Alfabético")]
        Alfabetico = 0,
        [Display(Name = @"Mayor facturación")]
        MayorFacturacion = 1,
        [Display(Name = @"Menor facturación")]
        MenorFacturacion = 2,
        [Display(Name = @"Mayor número de facturas")]
        MasFacturas = 3,
        [Display(Name = @"Menor número de facturas")]
        MenosFacturas = 5        
    }

}
