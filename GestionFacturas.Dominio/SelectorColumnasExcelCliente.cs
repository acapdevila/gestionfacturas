using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public class SelectorColumnasExcelCliente
{
    [Required]
    [Display(Name = "NIF")]
    public string LetraColumnaNumeroIdentificacionFiscal { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Nombre o Empresa")]
    public string LetraColumnaNombreOEmpresa { get; set; } = string.Empty;

    [Display(Name = "Nombre comercial")]
    public string LetraColumnaNombreComercial { get; set; } = string.Empty;

    [Display(Name = "Dirección")]
    public string LetraColumnaDireccion { get; set; } = string.Empty;

    [Display(Name = "Municipio")]
    public string LetraColumnaLocalidad { get; set; } = string.Empty;

    [Display(Name = "Provincia")]
    public string LetraColumnaProvincia { get; set; } = string.Empty;


    [Display(Name = "Código postal")]
    public string LetraColumnaCodigoPostal { get; set; } = string.Empty;

    [Display(Name = "E-mail")]
    public string LetraColumnaEmail { get; set; } = string.Empty;

    [Display(Name = "Persona de contacto")]
    public string LetraColumnaPersonaContacto { get; set; } = string.Empty;

    [Display(Name = "Nota interna")]
    public string LetraColumnaComentarioInterno { get; set; } = string.Empty;
}