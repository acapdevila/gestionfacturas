namespace GestionFacturas.Dominio.Infra
{
    public class Campo
    {
        public Campo()
        {
        }
        public Campo(string campo)
        {
            Nombre = campo;
        }

        public string Nombre { get; set; } = string.Empty;

        public string BuscarPor { get; set; } = string.Empty;
    }
}