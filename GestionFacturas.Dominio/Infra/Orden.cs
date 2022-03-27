namespace GestionFacturas.Dominio.Infra
{
    public class Orden
    {
        public const string Asc = "asc";
        public const string Desc = "desc";

        public Orden(string campo, string direccion)
        {
            Campo = campo.ToLower();

            if (string.IsNullOrEmpty(direccion)) return;

            direccion = direccion.ToLower();

            if (direccion != Asc && direccion != Desc)
                throw new Exception($"La dirección debe ser 'asc' o 'desc'. El valor '{direccion}' es incorrecto");

            Direccion = direccion;

        }

        public string Campo { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;
    }
}
