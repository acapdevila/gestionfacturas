using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;
using System.IO;

namespace GestionFacturas.Website.Helpers
{
    public static class FileImageHelper
    {
        private static string ConvertirNombreAGuid(this string nombreArchivo)
        {
            var result = Guid.NewGuid() + Path.GetExtension(nombreArchivo);
            return result;
        }

        private static string ObtenerUnNombreDeArchivoUnico(string nombreArchivo, string rutaCarpeta)
        {
            var folderFile = HostingEnvironment.MapPath("~" + rutaCarpeta);

            if (folderFile == null) throw new NullReferenceException(@"ObtenerUnNombreDeArchivoUnico: la variable rutaCarpeta es NULL");
         
            var newfilename = nombreArchivo;
         
                for (var i = 1; File.Exists(Path.Combine(folderFile, newfilename)); i++)
                {
                    newfilename = nombreArchivo.Insert(nombreArchivo.LastIndexOf('.'), "(" + i + ")");
                }

            return newfilename.Replace(".JPG", ".jpg").Replace(".GIF", ".gif").Replace(".PNG", ".png"); 
        }

        private static bool EsImagen(string file)
        {
            return file.EndsWith(".jpg", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".gif", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".png", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".JPG", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".GIF", StringComparison.CurrentCultureIgnoreCase) ||
                file.EndsWith(".PNG", StringComparison.CurrentCultureIgnoreCase);
        }

        public static void EliminarImagen(string rutaCarpeta, string nombreArchivo)
        {
            if (string.IsNullOrEmpty(rutaCarpeta) ||
                string.IsNullOrEmpty(nombreArchivo) ||
                !rutaCarpeta.Contains("/") ||
                nombreArchivo.Contains("/"))
                return;

            var rutaRelativaArchivo = string.Concat(rutaCarpeta, nombreArchivo);

           

            var filepath = HostingEnvironment.MapPath("~" + rutaRelativaArchivo);

            if (string.IsNullOrEmpty(filepath)) throw new NullReferenceException(@"EliminarImagen: la variable rutaRelativaArchivo es NULL");

            var theFile = new FileInfo(filepath);

            if (theFile.Exists)
            {
                File.Delete(filepath);
            }
                   
        }                

        public static string GuardarImagen(int maximumDimension,string  folderPath)
        {
            var image = ObtenerImagen();
       
            if (image != null && EsImagen(image.FileName))
            {
                var isWide = image.Width > image.Height;
                var bigestDimension = isWide ? image.Width : image.Height;
               
                if (bigestDimension > maximumDimension)
                {
                    if (isWide)
                        image.Resize(maximumDimension, ((maximumDimension * image.Height) / image.Width));
                    else
                        image.Resize(((maximumDimension * image.Width) / image.Height), maximumDimension);
                }
                string filename = ObtenerUnNombreDeArchivoUnico(Path.GetFileName(image.FileName), folderPath);

                image.Save(Path.Combine("~" + folderPath, filename));

                image = null;

                return filename;
            }

            return string.Empty;
        }

        private static WebImage ObtenerImagen()
        {
            var request = HttpContext.Current.Request;

            if (request.Files.Count == 0)
            {
                return null;
            }

            try
            {
                for (var i = 0; i < request.Files.Count; i++)
                {
                    var postedFile = request.Files[i];

                    if (string.IsNullOrEmpty(postedFile.FileName)) continue;

                    var image = new WebImage(postedFile.InputStream)
                    {
                        FileName = postedFile.FileName
                    };

                    return image;
                }

                return null;
            }
            catch
            {
                // The user uploaded a file that wasn't an image or an image format that we don't understand
                return null;
            }
        }

        private static IEnumerable<WebImage> ObtenerMultiplesImagenes(int maxLimit)
        {
            var request = HttpContext.Current.Request;
            var result = new List<WebImage>();

            if (request.Files.Count == 0)
            {
                return null;
            }

            var numberFilesToUpload = Math.Min(maxLimit, request.Files.Count);

            try
            {
                for (var i = 0; i < numberFilesToUpload; i++)
                {
                    var postedFile = request.Files[i];

                    if(string.IsNullOrEmpty(postedFile.FileName)) continue;

                    var image = new WebImage(postedFile.InputStream)
                    {
                        FileName = postedFile.FileName
                    };

                    result.Add(image);
                }

            }
            catch
            {
                // The user uploaded a file that wasn't an image or an image format that we don't understand
                return null;
            }
            return result;
        }
   
    }


    public abstract class CarpetaUploads
    {
        private const string Raiz = "/App_Data/";
        

        public const string Logos = Raiz + "Logos/";

        public const string Informes = Raiz + "Informes/";
        
        
    }
}
    
    
