using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web;

namespace NFD.ExceptionHandling
{
    /// <summary>
    ///  web层异常
    /// </summary>
    public class WebException : ApplicationException
    {
   

        /// <summary>
        /// 错误详情
        /// </summary>
        public ErrorInfo _errorInfo { get; set; }

        public WebException() { }
        /// <summary>
        /// web层异常
        /// </summary>
        /// <param name="ex">当前异常</param>
        /// <param name="errorPars">错误参数</param>
        /// <param name="vpars">页面变量或 GET POST参数 以键值存储</param>
        public WebException(Exception ex, ErrorParams errorPars, params VariableParams[] vpars)
        {
            _errorInfo = new ErrorInfo()
            {
                Method = errorPars.Method,
                AppendMessage = errorPars.AppendMessage,
                IpAddress = InternalMethod.GetIp,
                Message = InternalMethod.GetMessageByPars(ex, errorPars.AppendMessage, vpars),
                Source = ex.Source,
                Stack_trace = ex.StackTrace,
                Url = InternalMethod.GetUrl,
                Code = errorPars.Code
            };
        }
        public WebException(string message, Exception innerException)

            : base(message, innerException)
        {



        }
        protected WebException(SerializationInfo info, StreamingContext context)

            : base(info, context)
        {

        }



    }

}
