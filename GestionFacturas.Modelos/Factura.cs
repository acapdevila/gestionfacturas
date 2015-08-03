using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    public class Factura
    {
        public Factura()
        {
            Lineas = new List<LineaFactura>();
        }
        
        public int Id { get; set; }

        public int VendedorId { get; set; }
        public int CompradorId { get; set; }


        public string NumeroSerie { get; set; }
        public int NumeroFactura { get; set; }
        public string NumeroFormato { get; set; }
       
        public DateTime FechaEmisionFactura { get; set; }
        public DateTime FechaVencimientoFactura { get; set; }

        public int FormaPago { get; set; }
        public bool EstaEnviada { get; set; }
        public int  EstadoFactura { get; set; }
        public string Comentarios { get; set; }
        public string ComentariosPie { get; set; }
        
        public string RegistroCreadoPorUsuario { get; set; }
        public DateTime RegistroCreadoEnFecha { get; set; }
        public string RegistroEditadoPorUsuario { get; set; }
        public DateTime RegistroEditadoEnFecha { get; set; }
        
        public virtual FormaJuridica Vendedor { get; set; }
        public virtual FormaJuridica Comprador { get; set; }

        public virtual ICollection<LineaFactura> Lineas { get; set; }
        
    }

    public class LineaFactura
    {

    }
}
