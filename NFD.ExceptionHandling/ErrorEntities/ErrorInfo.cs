using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFD.ExceptionHandling
{
    public class ErrorInfo
    {
        /// <summary>
        /// 错误函数
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 出错url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// ip地址
        /// </summary>
        public string IpAddress { get; set; }
        /// <summary>
        /// 获取调用堆栈上直接帧的字符串表示形式
        /// </summary>
        public string Stack_trace { get; set; }
        /// <summary>
        /// 获取或设置导致错误的应用程序或对象的名称
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AppendMessage { get; set; }
        /// <summary>
        /// 错误编码
        /// </summary>
        public string Code { get; set; }
 
    }
}
