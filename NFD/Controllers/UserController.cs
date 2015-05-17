using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFD.Entities.Data;
using NFD.BLL;
using NFD.Entities.Common;
using COM.Extension;
using System.Data.Entity;
namespace NFD.Controllers
{
    public class UserController : Controller
    {
        //登录页
        public ActionResult Login()
        {
            return View();
        }

        //登录提交
        [ValidateInput(false)]
        public ActionResult LoginSubmit(UserInfo u)
        {

            var isLogin = UserManager.Login(ref u);
            if (isLogin)
            {
                return Redirect("/Home/Index");
            }
            else
            {
                ViewData[PubConst.VIEW_DATA_JS] = "用户名密码错误".ToAlert();
                return View("~/Views/User/Login.cshtml");
            }


        }

        //注销
        public ActionResult Logout()
        {
            UserManager.Logout();
            return RedirectToAction("Login");
        }
        //恶意操作跳转页面
        public ActionResult Safe()
        {
            return View();
        }
    }
}
