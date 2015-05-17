using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using COM.Utility;

namespace NFD.BLL
{
    /// <summary>
    /// 登录授权
    /// </summary>
    public class AuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 重写action处理前事件
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

           var isSafe= SafeValidates.CheckRequest(a =>
            {
                //请求带有危害参数进行页面处理
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.Redirect("/User/Safe");
            });
            
            string controllerName = GetControllerName(filterContext);
            switch (controllerName)
            {
                //无需登录验证
                case "user":
                case "error":
                    return;
                
                //需要登录验证
                default:
                    if (!UserManager.IsLogin && controllerName!="file")
                    {
                        Redirect(filterContext, "Default", new RouteValueDictionary(new { controller = "User", action = "Login" }));
                    }
                    break;
            }

        }
        #region  helper
        /// <summary>
        /// 登录验证失败
        /// </summary>
        /// <param name="filterContext"></param>
        public void Redirect(ActionExecutingContext filterContext, string routeName, RouteValueDictionary rvd)
        {
            filterContext.Result = new RedirectToRouteResult(routeName, rvd);
        }
        /// <summary>
        /// 获取请求的action (小写)
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public string GetActionName(ActionExecutingContext filterContext)
        {
            return filterContext.ActionDescriptor.ActionName.ToLower();
        }
        /// <summary>
        /// 获取请求的controller (小写)
        /// </summary>
        /// <param name="filterContext"></param>
        /// <returns></returns>
        public string GetControllerName(ActionExecutingContext filterContext)
        {
            return filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower();
        }
        #endregion

    }
}
