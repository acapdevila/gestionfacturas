using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    
    public class Cliente
    {
        public int Id { get; set; }
        
        public string NumeroIdentificacionFiscal { get; set; }

        public string NombreOEmpresa { get; set; }

        public string NombreComercial { get; set; }

        public string Direccion { get; set; }
        
        public string Localidad { get; set; }
        
        public string Provincia { get; set; }
        
        public string CodigoPostal { get; set; }
        
        public string Email { get; set; }

        public string PersonaContacto { get; set; }

        public string ComentarioInterno { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }

    }

    public class LineaListaGestionClientes
    {
        public int Id { get; set; }

        public string NumeroIdentificacionFiscal { get; set; }        

         public string NombreOEmpresa { get; set; }

        public string NombreComercial { get; set; }

        public string Email { get; set; }

        public int NumFacturas { get; set; }

        public string Nombre { get { return NombreComercial ?? NombreOEmpresa; } }

    }

    public class EditorCliente
    {
        public int Id { get; set; }

        [Display(Name = "NIF")]
        public string NumeroIdentificacionFiscal { get; set; }

        [Required]
        [Display(Name = "Nombre o Empresa")]
        public string NombreOEmpresa { get; set; }

        [Display(Name = "Nombre comercial")]
        public string NombreComercial { get; set; }

        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Municipio")]
        public string Localidad { get; set; }

        [Display(Name = "Provincia")]
        public string Provincia { get; set; }


        [Display(Name = "Código postal")]
        public string CodigoPostal { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Persona de contacto")]
        public string PersonaContacto { get; set; }

        [Display(Name = "Nota interna")]
        public string ComentarioInterno { get; set; }
    }

    public class SelectorColumnasExcelCliente
    {
        [Required]
        [Display(Name = "NIF")]
        public string LetraColumnaNumeroIdentificacionFiscal { get; set; }

        [Required]
        [Display(Name = "Nombre o Empresa")]
        public string LetraColumnaNombreOEmpresa { get; set; }

        [Display(Name = "Nombre comercial")]
        public string LetraColumnaNombreComercial { get; set; }

        [Display(Name = "Dirección")]
        public string LetraColumnaDireccion { get; set; }

        [Display(Name = "Municipio")]
        public string LetraColumnaLocalidad { get; set; }

        [Display(Name = "Provincia")]
        public string LetraColumnaProvincia { get; set; }


        [Display(Name = "Código postal")]
        public string LetraColumnaCodigoPostal { get; set; }

        [Display(Name = "E-mail")]
        public string LetraColumnaEmail { get; set; }

        [Display(Name = "Persona de contacto")]
        public string LetraColumnaPersonaContacto { get; set; }

        [Display(Name = "Nota interna")]
        public string LetraColumnaComentarioInterno { get; set; }
    }

}
