using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    public class Impuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Valor { get; set; }
    }
}
