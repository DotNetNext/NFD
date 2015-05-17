using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFD.Entities.Common
{
    public class BillPrice
    {
        /// <summary>
        /// 数量汇总
        /// </summary>
        public int allnum { get; set; }

        /// <summary>
        /// 成本单价汇总
        /// </summary>
        public decimal? allcostprice { get; set; }

        public decimal? allmaprice { get; set; }

        public decimal? allcuprice { get; set; }

        /// <summary>
        /// 成本价小计汇总
        /// </summary>
        public decimal? tolallcostprice { get; set; }

        public decimal? tolallmaprice { get; set; }

        public decimal? tolallcuprice { get; set; }

        public decimal? avg { get; set; }
    }
}
