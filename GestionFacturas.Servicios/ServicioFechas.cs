using System;

namespace GestionFacturas.Servicios
{
    public static class ServicioFechas
    {
        public static DateTime PrimerDiaMesAnterior()
        {
            return PrimerDiaMesActual().AddMonths(-1);
        }

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
