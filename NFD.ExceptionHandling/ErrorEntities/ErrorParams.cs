using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFD.ExceptionHandling
{
    /// <summary>
    /// 异常处理参数
    /// </summary>
    public class ErrorParams
    {
        /// <summary>
        /// 附加信息
        /// </summary>
        public string AppendMessage { get; set; }
        /// <summary>
        /// 错误 函数
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 错误编号
        /// </summary>
        public string Code { get; set; }
 
    }
}
