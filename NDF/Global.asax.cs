using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NFD.ExceptionHandling;

namespace NFD
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new NFD.BLL.AuthorizeAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            Safe.CheckRequest();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = HttpContext.Current.Server.GetLastError();
            if (ex is BasicException)
            {
                BasicException be = (BasicException)ex;
                if (be._exceptionType == ExceptionType.web)
                {
                    Response.Redirect("/Error/Web");
                }
                else if (be._exceptionType == ExceptionType.user)
                {
                    Response.Redirect("/Error/User");
                }
                return;
            }
            else if (ex is System.Web.HttpException)
            {
                Response.Redirect("/Error/http404");
            }
            else
            {
                Response.Redirect("/Error/http500");
            }

        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}