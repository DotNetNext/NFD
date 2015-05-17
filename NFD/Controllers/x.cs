//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using NFD.ExceptionHandling;
//using NFD.Entities.Data;
//using NFD.Entities;
//using NFD.BLL;
//using COM.Utility;
//using COM.Extension;
//namespace NFD.Controllers
//{
//    public class HomeController : Controller
//    {

//        #region  首页
//        //首页 
//        public ActionResult Index(string name, int total = 0, int pageIndex = 1, int pageSize = 10)
//        {
//           var KEY= System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("DFASFASFA", "md5");

//            HomeModel model = new HomeModel();
//            model.Home = HomeManager.GetHomeList(ref total, name, pageSize, pageIndex);
//            if (pageIndex != 1 && model.Home.Count == 0) { //如果页码不为1查询条数为0返回第一页
//                return this.Redirect("/");
//            }
//            model.PageString = PageStringHelper.PageString(total, pageSize, pageIndex, "/Home/Index?Name={0}&".ToFormat(Server.UrlEncode(name)));
//            return View(model);
//        }
//        //列表详情
//        public ActionResult Index_Detail(int id)
//        {
//            Home model = HomeManager.GetHomeById(id);
//            return View(model);
//        }

//        //保存页（编辑/添加）
//        [HttpGet]
//        public ActionResult Index_Save(int id = 0)
//        {
//            Home model = new Home();
//            if (id > 0)
//            {
//                model = HomeManager.GetHomeById(id);
//            }
//            return View(model);
//        }
//        //保存页提交（编辑/添加）
//        [HttpPost]
//        public ActionResult Index_Save(Home home, string urlReferrer)
//        {
//            try
//            {
//                HomeManager.Save(home);
//                return this.Redirect(urlReferrer);
//            }
//            catch (Exception ex)
//            {
//                //异常处理
//                ErrorParams pars = new ErrorParams()
//                {
//                    AppendMessage = "保存home出错",
//                    Code =ex.GetHashCode().ToString(),
//                    Method = "homeController.Index_Delete"
//                };
//                //页面参数（可以不传）
//                var vps = new VariableParams[]{
//                        new VariableParams(){ Key="name", Value=home.Name},
//                        new VariableParams(){ Key="Content", Value=home.Content}
//                    };
//                throw new WebException(ex, pars, vps);
//            }
//        }
//        //删除
//        [HttpGet]
//        public ActionResult Index_Delete(string name, int id = 0)
//        {
//            try
//            {
//                HomeManager.Delete(id);
//                return this.Redirect(Request.UrlReferrer.ToString());
//            }
//            catch (Exception ex)
//            {

//                //异常处理
//                ErrorParams pars = new ErrorParams()
//                {
//                    AppendMessage = "删除home出错 id为".ToFormat(id),
//                    Code =ex.GetHashCode().ToString(),
//                    Method = "homeController.Index_Delete"
//                };
//                throw new WebException(ex, pars);
//            }
//        }


//        #endregion

//        #region 页面2

//        public ActionResult Index2()
//        {
//            return View();
//        }
//        #endregion


//    }
//}
