using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio;

public enum FormaPagoEnum
{
    [Display(Name = @"Sin definir")]
    SinDefinir = 0,
    Transferencia = 1,
    Tarjeta = 2,
    Efectivo = 3,
    [Display(Name = @"Domiciliación")]
    Domiciliacion = 4
}