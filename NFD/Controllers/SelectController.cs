using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NFD.Controllers
{
    public class SelectController : Controller
    {

        //获了下拉列表值
        [HttpGet]
        public JsonResult Trader()
        {
            var reval = BLL.TraderManager.GetList().Select(c => new { key = c.trader_id, val = c.name }).OrderByDescending(c => c.key).ToList();
            return Json(reval, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Factory()
        {
            var reval = BLL.DictManager.GetFactory().Select(c => new { key = c.dd_id, val = c.d_name }).OrderByDescending(c => c.key).ToList();
            return Json(reval, JsonRequestBehavior.AllowGet);
        }

    }
}
