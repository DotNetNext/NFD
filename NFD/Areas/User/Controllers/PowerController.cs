using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
using NFD.BLL;
using NFD.Entities.Common;
namespace NFD.Areas.User.Controllers
{
    public class PowerController : Controller
    {

        public ActionResult Index(int id)
        {
            ViewBag.userId = id;
            return View();
        }

        public ActionResult SavePower(string sp, int userId)
        {
            UserManager.SaveUserMenuIds(userId, sp);
            TempData[PubConst.VIEW_DATA_JS] = "操作成功！".ToAlert();
            return RedirectToAction("Index","Account");
        }

    }
}
