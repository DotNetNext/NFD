using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.Exception
{
    [Serializable]

    public class WebException : ApplicationException
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        private Exception _ex { get; set; }
        public WebException() { }


        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="sql">sql</param>
        /// <param name="exception">exception</param>
        /// <param name="pars">SqlParameter[] 可以不传</param>
        public WebException(Exception exception, string appendMessage, Dictionary<object, object> pars)
        {
            StringBuilder sbMessage = new StringBuilder();
            SetException(sbMessage, exception, appendMessage, pars);
            throw _ex;

        }


        public WebException(string message, Exception innerException)

            : base(message, innerException)
        {



        }

        protected WebException(SerializationInfo info, StreamingContext context)

            : base(info, context)
        {

        }
        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="sbMessage"></param>
        /// <param name="exception"></param>
        /// <param name="sql"></param>
        /// <param name="pars"></param>
        private void SetException(StringBuilder sbMessage, Exception exception, string appendMessage, Dictionary<object, object> pars)
        {
            if (appendMessage != null)
            {
                sbMessage.Append(GetErrorMessage("错误提示", appendMessage));
            }

            sbMessage.Append(GetErrorMessage("message", exception.Message));
            if (exception.InnerException != null && !string.IsNullOrEmpty(exception.Message))
            {
                sbMessage.Append(GetErrorMessage("Inner message", exception.InnerException.Message));
            }
            if (pars != null && pars.Count > 0)
            {
                foreach (var r in pars)
                {
                    string parMessage = string.Format("参数【{1}={0}】\n", r.Key, r.Value);
                    sbMessage.Append(parMessage);
                }

            }
            sbMessage.Append(GetErrorMessage("TargetSite", exception.TargetSite));
            _ex = new Exception(sbMessage.ToString(), exception.InnerException)
            {
                HelpLink = exception.HelpLink,
                Source = exception.Source
            };
        }

        /// <summary>
        /// 获取异常信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private string GetErrorMessage(string key, object val)
        {
            return key + ":" + val + "\n";
        }
    }
}
