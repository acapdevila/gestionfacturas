using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionFacturas.Modelos
{
    public class MensajeEmail
    {
        public MensajeEmail()
        {
            DireccionesDestinatarios = new List<string>();
            Adjuntos = new List<ArchivoAdjunto>();
        }

        public string NombreRemitente { get; set; }
        public string DireccionRemitente { get; set; }

        public List<string> DireccionesDestinatarios { get; set; }

      

        public string Asunto { get; set; }

        public string Cuerpo { get; set; }

        public List<ArchivoAdjunto> Adjuntos { get; set; }


    }

    public class ArchivoAdjunto
    {
        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }

        public string MimeType { get; set; }
    }
}
