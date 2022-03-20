namespace GestionFacturas.Dominio
{
    public class FormaJuridica
    {
        public string NumeroIdentificacionFiscal { get; set; } = string.Empty;
        public string NombreOEmpresa { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Localidad { get; set; } = string.Empty;
        public string Provincia { get; set; } = string.Empty;
        public string CodigoPostal { get; set; } = string.Empty;
    }

    public class PersonaFisica : FormaJuridica
    {
        public string Apellidos { get; set; } = string.Empty;
        public string Nombre { get { return string.Format("{0} {1}", NombreOEmpresa, Apellidos).Trim(); } }
    }

    public class PersonaJuridica : FormaJuridica
    {
        public string NombreComercial { get; set; } = string.Empty;
        public string Nombre { get { return (NombreComercial ?? NombreOEmpresa).Trim(); } }
    }

}
