using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFD.Entities.Common
{
    /// <summary>
    /// 公用枚举
    /// </summary>
    public class PubEnum
    {
        /// <summary>
        /// 附件类型
        /// </summary>
        public enum AttaType
        {
            手织样 = 0
        }
        /// <summary>
        /// 报价单状态
        /// </summary>
        public enum HtStatus
        {
            登记 = 0,
            放大样 = 1,
            产样衣 = 2,
            放大货 = 3,
            报价 = 4,
            登记作废 = -1,
            放样作废 = -2,
            样衣作废 = 3
        }
    }
}
