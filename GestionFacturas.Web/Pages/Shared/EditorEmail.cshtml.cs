using System.ComponentModel.DataAnnotations;

namespace GestionFacturas.Web.Pages.Shared;

public class EditorEmail
{
    [Display(Name = @"De")]
    [Required(ErrorMessage = @"Escribe la dirección de e-mail del remitente")]
    [RegularExpression(
        @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$",
        ErrorMessage = "Verifica tu e-mail.")]
    public string Remitente { get; set; } = string.Empty;

    [Display(Name = @"Para")]
    [Required(ErrorMessage = @"Escribe la dirección de e-mail del destinatario")]
    [RegularExpression(@"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$", ErrorMessage = "Verifica tu e-mail.")]
    public string Destinatario { get; set; } = string.Empty;

    [Display(Name = @"Asunto")]
    [Required(ErrorMessage = @"Escribe el asunto")]
    public string Asunto { get; set; } = string.Empty;

    [Display(Name = @"Mensaje")]
    public string ContenidoHtml { get; set; } = string.Empty;


}