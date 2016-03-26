using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionFacturas.Modelos;
using System.IO;

namespace GestionFacturas.Servicios
{
    public class ServicioEmail
    {
        public async Task EnviarMensaje(MensajeEmail mensaje)
        {
            Validar(mensaje);

            var email = GenerarEmail(mensaje);

            foreach (var adjunto in mensaje.Adjuntos)
            {
                email.AdjuntarArchivo(adjunto);
            }           
                    
            await Enviar(email);
        }        

        private MailMessage GenerarEmail(MensajeEmail mensaje)
        {
            var email = new MailMessage
            {
                Subject = mensaje.Asunto,
                From = new MailAddress(mensaje.DireccionRemitente, mensaje.NombreRemitente),
                Body = mensaje.Cuerpo
            };

           email.ReplyToList.Add(mensaje.DireccionRemitente);

            foreach (var destinatario in mensaje.DireccionesDestinatarios)
            {
                email.To.Add(destinatario);
            }

            return email;
        }

        private void Validar(MensajeEmail mensaje)
        {
            if (string.IsNullOrEmpty(mensaje.DireccionRemitente))
                throw new ArgumentException("No se ha indicado el remitente", "DireccionRemitente");

            if (!mensaje.DireccionesDestinatarios.Any())
                throw new ArgumentException("No se ha indicado ningún destinatario", "DireccionesDestinatarios");
        }


        private static async Task Enviar(MailMessage message)
        {
            var client = new SmtpClient();
            await client.SendMailAsync(message);
        }
    }
}
