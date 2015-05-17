using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.ExceptionHandling;
using NFD.Entities.Data;
using NFD.Entities;
using NFD.BLL;
using COM.Utility;
using COM.Extension;
namespace NFD.Controllers
{
    public class HomeController : Controller
    {

        //首页
        public ActionResult Index(int total=0, int pageIndex=1,int pageSize=10)
        {
            HomeModel model = new HomeModel();
            model.Ttile = "首页";
            model.Home = HomeManager.GetHomeList(ref total, pageSize, pageIndex);
            model.language = new List<string> { ".net", "java", "js" };
            model.PageString = PageStringHelper.PageString(total, pageSize, pageIndex, "/Home/Index");
            return View(model);
        }



        //用户异常
        public ActionResult UserEx()
        {
            throw new UserException(new Exception("用户错误"), "用于测试用户异常", new Dictionary<object, object>());
        }
        //web异常
        public ActionResult WebEx()
        {
            throw new WebException(new Exception("web错误"), "用于测试用户异常", new Dictionary<object, object>());
        }
    }
}
