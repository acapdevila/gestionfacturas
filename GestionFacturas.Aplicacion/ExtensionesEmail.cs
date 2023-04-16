using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GestionFacturas.Dominio;

namespace GestionFacturas.Aplicacion
{
    public static class ExtensionesEmail
    {

     

        public static void AdjuntarArchivo(this MailMessage email, ArchivoAdjunto archivoAdjunto)
        {
            var stream = new MemoryStream(archivoAdjunto.Archivo) { Position = 0 };

            // Create  the file attachment for this e-mail message.
            var data = new Attachment(stream, archivoAdjunto.Nombre, archivoAdjunto.MimeType);
            // Add time stamp information for the file.
            var disposition = data.ContentDisposition;
            disposition!.CreationDate = DateTime.Now;
            disposition.DispositionType = archivoAdjunto.MimeType;
            disposition.Size = stream.Length;

            email.Attachments.Add(data);
        }
    }
}
