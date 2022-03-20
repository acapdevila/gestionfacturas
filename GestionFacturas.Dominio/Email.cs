namespace GestionFacturas.Dominio
{
    public class MensajeEmail
    {
        public MensajeEmail()
        {
            DireccionesDestinatarios = new List<string>();
            Adjuntos = new List<ArchivoAdjunto>();
        }

        public string NombreRemitente { get; set; } = string.Empty;
        public string DireccionRemitente { get; set; } = string.Empty;

        public List<string> DireccionesDestinatarios { get; set; }

      

        public string Asunto { get; set; } = string.Empty;

        public string Cuerpo { get; set; } = string.Empty;

        public List<ArchivoAdjunto> Adjuntos { get; set; } = new ();


    }
}
