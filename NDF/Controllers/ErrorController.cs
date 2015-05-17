using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
 

namespace NFD.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult Web()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

        public ActionResult http404()
        {
            return View();
        }
        public ActionResult http500()
        {
            return View();
        }

    }
}
