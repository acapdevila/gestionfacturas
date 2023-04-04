namespace GestionFacturas.Dominio.Infra
{
   public  static class ExtensionesFecha
    {
        private const string FormatoFecha = "dd/MM/yyyy";
        public static string ToFechaCorta(this DateTimeOffset fecha)
        {
            return fecha.ToString(FormatoFecha);
        }

        public static string ToFechaCorta(this DateTime fecha)
        {
            return fecha.ToString(FormatoFecha);
        }

        public static string ToFechaCorta(this DateOnly fecha)
        {
            return fecha.ToString(FormatoFecha);
        }
        public static string ToInputDate(this DateOnly fecha)
        {
            return fecha.ToString("yyyy-MM-dd");
        }
        public static string ToInputDate(this DateTime fecha)
        {
            return fecha.ToString("yyyy-MM-dd");
        }
        public static DateOnly FromInputToDateOnly(this string inputFecha, DateOnly defaultValue = default)
        {
            if (string.IsNullOrEmpty(inputFecha)) 
                    return defaultValue;
            return DateOnly.Parse(inputFecha);
        }

        public static DateTime FromInputToDateTime(this string inputFecha, DateTime defaultValue = default)
        {
            if (string.IsNullOrEmpty(inputFecha))
                return defaultValue;
            _ = DateTime.TryParse(inputFecha, out defaultValue);
            return defaultValue;
        }

        public static string ToFechaYHora(this DateTimeOffset fecha)
        {
            return fecha.ToString($"{FormatoFecha} HH:mm");
        }
    }
}
