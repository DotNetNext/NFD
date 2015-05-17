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

             return View(TempData["errorInfo"]);  
        }

        public ActionResult User()
        {
            return View(TempData["errorInfo"]);
        }

        public ActionResult http404()
        {
            return View(TempData["errorInfo"]);
        }
        public ActionResult http500()
        {
            return View(TempData["errorInfo"]);
        }
     

    }
}
