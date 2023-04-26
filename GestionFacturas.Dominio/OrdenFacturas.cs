
using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Dominio
{
    
    public enum OrdenFacturas
    {
        //[Display(Name = @"Número")]
        //NumeroDesc = 0,
        //[Display(Name = @"Número (antiguas)")]
        //NumeroAsc = 1,
        [Display(Name = @"Fecha")]
        FechaDesc = 2,
        [Display(Name = @"Fecha (antiguas)")]
        FechaAsc = 3,

    }
}
