using System;
using System.Web;

namespace GestionFacturas.Website.Helpers
{
    public abstract class RouteDataKey
    {
        // public const string Town = "town";
        public const string Lang = "lang";
        public const string Id = "id";
        public const string Slug = "slug";
        public const string Category = "category";
        public const string Controller = "controller";
        public const string Action = "action";
        public const string Area = "area";
    }

    public abstract class RouteName
    {
        public const string Facturas = "Facturas";
        public const string Admin = "Manage";
        public const string Account = "Account";
    }


    public static class RouteServer
    {
        public static string GetUrlWebRoot()
        {
           // return String.Format("{0}://{1}{2}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Headers["host"], HttpContext.Current.Request.ApplicationPath);

           return  string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host,
                                         HttpContext.Current.Request.Url.Port == 80
                                          ? string.Empty
                                          : ":" + HttpContext.Current.Request.Url.Port,
                                     HttpContext.Current.Request.ApplicationPath);

        }

    }
}
