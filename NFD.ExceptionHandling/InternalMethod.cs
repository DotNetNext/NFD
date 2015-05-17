using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
namespace NFD.ExceptionHandling
{
    internal class InternalMethod
    {

        #region  公用函数
        /// <summary>
        /// 创建新的Exception并将 Exception中message信息进行完善
        /// </summary>
        /// <param name="appendMessage">追加信息</param>
        /// <param name="exception">传入异常对像</param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        public static string GetMessageByPars(Exception exception, string appendMessage, params VariableParams[] pars)
        {
            //格式化单行错误消息
            Func<string, object, object> GetErrorMessage = (key, val) =>
            {
                return key + ":" + val + "\n";
            };

            //声名返回值
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.Append(GetErrorMessage("错误信息", exception.Message));
            if (appendMessage != null)
            {
                sbMessage.Append(GetErrorMessage("追加信息", appendMessage));
            }
            if (exception.InnerException != null && !string.IsNullOrEmpty(exception.Message))
            {
                sbMessage.Append(GetErrorMessage("内部错误信息", exception.InnerException.Message));
            }
            if (pars != null && pars.Length > 0)
            {
                foreach (var r in pars)
                {
                    string parMessage = string.Format("参数【{0}={1}】\n", r.Key, r.Value);
                    sbMessage.Append(parMessage);
                }

            }
            sbMessage.Append(GetErrorMessage("当前异常的方法", exception.TargetSite));
            sbMessage.Append(GetErrorMessage("导致错误的应用程序或对象的名称", exception.Source));
            sbMessage.Append(GetErrorMessage("堆栈信息", exception.StackTrace));
            return sbMessage.ToString();
        }
        public static string GetIp
        {
            get
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
                    return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].Split(new char[] { ',' })[0];
                else
                    return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
        }
        /// <summary>
        /// 当前 HTTP 请求的完整 URL （包含参数信息）
        /// </summary>
        public static string GetUrl
        {
            get
            {
                return (HttpContext.Current.Request.ServerVariables["URL"] + "?" + HttpContext.Current.Request.ServerVariables["QUERY_STRING"]);
            }
        }
        #endregion
    }
}
