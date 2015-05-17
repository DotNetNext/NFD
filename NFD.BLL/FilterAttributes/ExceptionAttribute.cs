using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using NFD.ExceptionHandling;
using System.Web.Routing;

namespace NFD.BLL
{

    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionAttribute : FilterAttribute, IExceptionFilter   //HandleErrorAttribute
    {
        public void OnException(ExceptionContext filterContext)
        {

            
        }
    }
}
