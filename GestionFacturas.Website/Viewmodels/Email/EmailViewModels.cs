using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GestionFacturas.Website.Viewmodels.Email
{
    public class EditorEmail
    {
        [Display(Name = @"De")]
        [Required(ErrorMessage = @"Escribe la dirección de e-mail del remitente")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Verifica tu e-mail.")]
        public string Remitente { get; set; }
         
        [Display(Name = @"Para")]
        [Required(ErrorMessage = @"Escribe la dirección de e-mail del destinatario")]
        [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Verifica tu e-mail.")]
        public string Destinatario { get; set; }

        [Display(Name = @"Asunto")]
        [Required(ErrorMessage = @"Escribe el asunto")]
        public string Asunto { get; set; }

        [AllowHtml]
        [Display(Name = @"Mensaje")]
        public string ContenidoHtml { get; set; }
        

    }

    
}
