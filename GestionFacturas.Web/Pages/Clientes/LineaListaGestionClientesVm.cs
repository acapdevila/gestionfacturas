namespace GestionFacturas.Web.Pages.Clientes;

public class LineaListaGestionClientesVm
{
    public int Id { get; set; }

    public string NumeroIdentificacionFiscal { get; set; } = string.Empty;

    public string NombreOEmpresa { get; set; } = string.Empty;

    public string NombreComercial { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int NumFacturas { get; set; }

    public string Nombre { get { return NombreComercial ?? NombreOEmpresa; } }

    public string Direccion { get; set; } = string.Empty;

}