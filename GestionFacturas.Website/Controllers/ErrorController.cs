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
    }
}
