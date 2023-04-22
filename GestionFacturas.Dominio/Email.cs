using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace GestionFacturas.Dominio
{
    public class MensajeEmail
    {
        public const string EmailRegEx =
            @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";

        public MensajeEmail(
            string nombreRemitente, 
            string direccionRemitente,
            string asunto,
            string cuerpo)
        {
           Asunto = asunto;
           Cuerpo = cuerpo;
           NombreRemitente = nombreRemitente;
           DireccionRemitente = direccionRemitente;
           
        }

        public string NombreRemitente { get;} 
        public string DireccionRemitente { get; }


        private readonly List<string> _direccionesDestinatarios = new ();
       
        public IReadOnlyList<string> DireccionesDestinatarios() => _direccionesDestinatarios.ToList();


        public string Asunto { get;  } 

        public string Cuerpo { get; } 

        
        public List<ArchivoAdjunto> Adjuntos { get; init; } = new ();

        public Result AñadirDestinatarios(List<string> direccionesDestinatarios)
        {
            var validacion = Result.Combine(direccionesDestinatarios.Select(ValidarDireccionEmail));
            if (validacion.IsFailure) return validacion;
            
            _direccionesDestinatarios.AddRange(direccionesDestinatarios);

            return Result.Success();
        }

        public static Result ValidarDireccionEmail(string direccionDestinatario)
        {
            var defaultRegex = new Regex(EmailRegEx);

            var match = defaultRegex.Match(direccionDestinatario);

            return !match.Success ? 
                Result.Failure($"Dirección email no valida: {direccionDestinatario}") : 
                Result.Success();
        }
    }
}
