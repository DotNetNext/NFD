using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.BLL.Bill;

namespace NFD.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //ChanceBillExportManager.Export(2,DateTime.Now,DateTime.Now);
            return View();
        }

    }
}
