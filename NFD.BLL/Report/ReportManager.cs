using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using NFD.Entities.Data;
using COM.Utility;
using System.Data;
using COM.Extension;
namespace NFD.BLL.Report
{
    /// <summary>
    /// 报表中心
    /// </summary>
    public class ReportManager
    {
        /// <summary>
        /// 获取订单汇总表
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_Report_Order> GetV_Report_OrderList(NFDEntities db)
        {
            return db.V_Report_Order;
        }


        public static void Export()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("客户"));
            dt.Columns.Add(new DataColumn("款号"));
            dt.Columns.Add(new DataColumn("订单数量"));
            dt.Columns.Add(new DataColumn("面料到货数量"));
            dt.Columns.Add(new DataColumn("单耗"));
            dt.Columns.Add(new DataColumn("面料单价"));
            dt.Columns.Add(new DataColumn("面料金额"));
            dt.Columns.Add(new DataColumn("出货数量"));
            dt.Columns.Add(new DataColumn("加工费单价"));
            dt.Columns.Add(new DataColumn("成品金额"));
            dt.Columns.Add(new DataColumn("辅料金额"));
            dt.Columns.Add(new DataColumn("检品费(元)"));
            dt.Columns.Add(new DataColumn("合同单价 $"));
            dt.Columns.Add(new DataColumn("金额 $"));
            dt.Columns.Add(new DataColumn("收汇时间"));
            dt.Columns.Add(new DataColumn("汇率"));
            dt.Columns.Add(new DataColumn("人民币"));
            dt.Columns.Add(new DataColumn("快件费"));
            dt.Columns.Add(new DataColumn("进口报关费"));
            dt.Columns.Add(new DataColumn("出口报关费"));
            dt.Columns.Add(new DataColumn("备注"));
            using (NFDEntities db = new NFDEntities())
            {
                List<V_Report_Order> orderList = GetV_Report_OrderList(db).OrderByDescending(c => c.create_time).ToList();
                foreach (var r in orderList)
                {
                    var dr = dt.NewRow();
                    var index = orderList.IndexOf(r);
                    dr["客户"] = r.trader_name;
                    dr["款号"] = r.clothing_number;
                    dr["订单数量"] = r.num.ToMoneyString();
                    dr["面料到货数量"] = r.arrival_num.ToMoneyString();
                    dr["单耗"] = r.consumption.ToMoneyString();
                    dr["面料单价"] = r.fabric_price.ToMoneyString();
                    dr["面料金额"] = r.fabric_total_price.ToMoneyString();
                    dr["出货数量"] = r.delivers_num.ToMoneyString();
                    dr["加工费单价"] = r.pricessing_fee.ToMoneyString();
                    dr["成品金额"] = r.reality_total_price.ToMoneyString();
                    dr["辅料金额"] = r.my_accesssor.ToMoney()+r.factory_accesssor.ToMoney();
                    dr["检品费(元)"] = r.inspection_fee.ToMoney();
                    dr["合同单价 $"] = (r.contract_price/r.rate).ToMoneyString();
                    dr["金额 $"] = (r.contract_price_total/r.rate).ToMoneyString();
                    dr["收汇时间"] = r.get_price_date.ToDateStr("yyyy-MM-dd");
                    dr["汇率"] = r.rate.ToMoneyString();
                    dr["人民币"] = r.contract_price_total.ToMoney(); 
                    dr["快件费"] = r.single_fee.ToMoneyString();
                    dr["出口报关费"] = r.export_price.ToMoneyString();
                    dr["进口报关费"] = r.wellhead_price.ToMoneyString();
                    dr["备注"] = "";
                    dt.Rows.Add(dr);
                }
                COM.Utility.AsposeExcel.MergeCellExport(dt,"订单汇总表.xls");

            }

        }
    }
}
