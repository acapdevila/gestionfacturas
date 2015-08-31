using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Servicios
{
    public static class ServicioFechas
    {
        public static DateTime PrimerDiaMesActual()
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        }
        public static DateTime UltimoDiaMesActual()
        {
           return PrimerDiaMesActual().AddMonths(1).AddDays(-1);
        }

    }
}
