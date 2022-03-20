namespace GestionFacturas.Dominio
{
    public class Impuesto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }
}
