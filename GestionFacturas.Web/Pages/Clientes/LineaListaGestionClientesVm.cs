namespace GestionFacturas.Web.Pages.Clientes;

public class LineaListaGestionClientesVm
{
    public int Id { get; set; }
    public string Nif { get; set; } = string.Empty;

    public string NombreOEmpresa { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;

    public int NumFacturas { get; set; }

}