namespace GestionFacturas.Dominio.Infra
{
    public static class FuncionesFechas
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
