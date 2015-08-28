using System;
using System.Web.Mvc;
using System.Net;
using Microsoft.Owin.Logging;

namespace Bn.LookAuto.Website.Controllers.Common
{
    public class ErrorController : Controller
    {
        public ActionResult Test()
        {
            //Logger.Info("Log test provocado");
            throw new Exception("Error test provocado");
        }

        public ActionResult InternalError500()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return View("InternalError500");
        }

        public ActionResult Forbidden403()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return View("Forbidden403");
        }

        public ViewResult NotFound404(string path)
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return View("NotFound404");
        }

        public ViewResult BadRequest400()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return View("BadRequest400");
        }


    }
}
