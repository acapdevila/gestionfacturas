namespace GestionFacturas.Dominio;

public class ArchivoAdjunto
{
    public string Nombre { get; set; } = string.Empty;
    public byte[] Archivo { get; set; } = { };

    public string MimeType { get; set; } = string.Empty;
}