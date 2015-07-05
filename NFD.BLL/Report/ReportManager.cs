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
            dt.Columns.Add(new DataColumn("订单数量（套）"));
            dt.Columns.Add(new DataColumn("面料订货数（米）"));
            dt.Columns.Add(new DataColumn("单耗"));
            dt.Columns.Add(new DataColumn("面料到货数(米)"));
            dt.Columns.Add(new DataColumn("面料单价(元)"));
            dt.Columns.Add(new DataColumn("面料金额(元)"));
            dt.Columns.Add(new DataColumn("裁剪数(套)"));
            dt.Columns.Add(new DataColumn("送检数（套）"));//

            dt.Columns.Add(new DataColumn("出货数量（套）"));
            dt.Columns.Add(new DataColumn("B品数量"));

            dt.Columns.Add(new DataColumn("加工费单价（元）"));
            dt.Columns.Add(new DataColumn("成品金额（元）"));
            dt.Columns.Add(new DataColumn("辅料金额（元）"));
            dt.Columns.Add(new DataColumn("检品费(元)"));
            dt.Columns.Add(new DataColumn("合同单价 $"));
            dt.Columns.Add(new DataColumn("金额 $"));
            dt.Columns.Add(new DataColumn("收汇时间"));
            dt.Columns.Add(new DataColumn("汇率"));
            dt.Columns.Add(new DataColumn("人民币"));
            dt.Columns.Add(new DataColumn("快件费(元)"));
            dt.Columns.Add(new DataColumn("进口报关费(元)"));
            dt.Columns.Add(new DataColumn("出口报关费(元)"));
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
                    dr["订单数量（套）"] = r.num.ToMoneyString();
                    dr["面料订货数（米）"] = r.fabric_order_num;
                    dr["面料到货数(米)"] = r.arrival_num.ToMoneyString();
                    if (r.num > 0 && r.fabric_order_num > 0)
                    {
                        dr["单耗"] = (Convert.ToDecimal(r.fabric_order_num) / Convert.ToDecimal(r.num)).ToMoneyString();
                    }
                    dr["面料单价(元)"] = r.fabric_price.ToMoneyString();
                    dr["面料金额(元)"] = r.fabric_total_price.ToMoneyString();
                    dr["裁剪数(套)"] = r.actual_num_tol.ToMoneyString();

                    dr["出货数量（套）"] = r.delivers_num.ToMoneyString();
                    dr["B品数量"] = r.bnum;

                    dr["加工费单价（元）"] = r.pricessing_fee.ToMoneyString();
                    dr["成品金额（元）"] = r.reality_total_price.ToMoneyString();
                    dr["辅料金额（元）"] = r.my_accesssor.ToMoney();
                    dr["检品费(元)"] = r.inspection_fee.ToMoney();
                    dr["合同单价 $"] = r.contract_price.ToMoneyString();
                    dr["金额 $"] = r.contract_price_total.ToMoneyString();
                    dr["收汇时间"] = r.get_price_date.ToDateStr("yyyy-MM-dd");
                    dr["汇率"] = r.rate.ToMoneyString();
                    dr["人民币"] = (r.contract_price_total * r.rate).ToMoneyString();
                    dr["快件费(元)"] = r.single_fee.ToMoneyString();
                    dr["出口报关费(元)"] = r.export_price.ToMoneyString();
                    dr["进口报关费(元)"] = r.wellhead_price.ToMoneyString();
                    dr["备注"] = "";
                    dt.Rows.Add(dr);
                }
                var toldr = dt.NewRow();
                toldr["客户"] = "统计：";
                toldr["面料金额(元)"] = orderList.Select(c => c.fabric_total_price).Sum().ToMoneyString();
                toldr["金额 $"] = orderList.Select(c => c.contract_price_total).Sum().ToMoneyString();
                toldr["人民币"] = orderList.Select(c => (c.contract_price_total * c.rate)).Sum().ToMoneyString();
                toldr["加工费单价（元）"] = orderList.Select(c => c.pricessing_fee).Sum().ToMoneyString();
                toldr["快件费(元)"] = orderList.Select(c => c.single_fee).Sum().ToMoneyString();
                toldr["出口报关费(元)"] = orderList.Select(c => c.export_price).Sum().ToMoneyString();
                toldr["进口报关费(元)"] = orderList.Select(c => c.wellhead_price).Sum().ToMoneyString();
                dt.Rows.Add(toldr);
                COM.Utility.AsposeExcel.MergeCellExport(dt, "订单汇总表.xls");

            }

        }
    }
}
