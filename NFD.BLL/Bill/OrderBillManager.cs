using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
using NFD.Entities.Common;
using System.Data;
using System.Data.Objects.SqlClient;
namespace NFD.BLL.Bill
{
    public class OrderBillManager
    {
        /// <summary>
        /// 保存定单
        /// </summary>
        /// <param name="bill"></param>
        public static void SaveOrderBill(Entities.Data.OrderBill bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (bill.o_id.IsZero())//添加
                {
                    bill.create_time = DateTime.Now;
                    bill.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    bill.creator_name = UserManager.GetCurrentUserInfo.userName;
                    bill.factory_id = bill.factory_name.ToInt();
                    if (bill.chance_id > 0)
                    {
                        var chance = db.ChanceBill.Single(c => c.bill_id == bill.chance_id);
                        chance.status = 1;
                    }
                    db.OrderBill.AddObject(bill);
                }
                else //编辑
                {

                    bill.modified_time = DateTime.Now;
                    bill.factory_id = bill.factory_name.ToInt();
                    db.Update<OrderBill>(bill.o_id, new
                    {
                        num = bill.num,
                        order_date = bill.order_date,
                        submission_date = bill.submission_date,
                        styles = bill.styles,
                        trader_id = bill.trader_id,
                        no = bill.no,
                        clothing_number = bill.clothing_number,
                        get_date = bill.get_date,
                        factory_name = bill.factory_name,
                        rate = bill.rate,
                        custome_price = bill.custome_price,
                        market_price = bill.market_price,
                        factory_id = bill.factory_id,
                        cost_price = bill.cost_price,
                        modified_time = DateTime.Now,
                        wellhead_price = bill.wellhead_price,
                        export_price = bill.export_price,
                        get_price_date = bill.get_price_date,
                        inspection_fee = bill.inspection_fee,
                        pricessing_fee=bill.pricessing_fee,
                       ship_date= bill.ship_date


                    });
                }

                db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取报价单列表
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<OrderBill> GetOrderList(NFDEntities db)
        {
            return db.OrderBill.Where(c => (c.is_del == false || c.is_del == null));
        }

        /// <summary>
        /// 获取订单根据订单ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static OrderBill GetOrderById(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var reval = db.OrderBill.Single(c => c.o_id == id);
                return reval;
            }
        }

        /// <summary>
        /// 删除定单
        /// </summary>
        /// <param name="oId"></param>
        /// <returns></returns>
        public static bool DelOrder(int oId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.FalseDelete<OrderBill>(oId);
            }
        }

        public static BillPrice GetTolByPars(OrderBill bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var orderQueryable = GetOrderList(db);
                if (bill.trader_id > 0)
                {
                    orderQueryable = orderQueryable.Where(c => c.trader_id == bill.trader_id);
                }
                if (bill.factory_name.IsHasValue())
                {
                    orderQueryable = orderQueryable.Where(c => c.factory_name.Contains(bill.factory_name));
                }

                if (bill.clothing_number.IsHasValue())
                {
                    orderQueryable = orderQueryable.Where(c => c.clothing_number.Contains(bill.clothing_number));
                }
                if (bill.chance_id > 0)
                {
                    orderQueryable = orderQueryable.Where(c => c.chance_id == bill.chance_id);
                }

                if (bill.no.IsHasValue())
                {
                    orderQueryable = orderQueryable.Where(c => c.no.Contains(bill.no));
                }
                if (orderQueryable.Count() == 0) return new BillPrice();
                return new BillPrice()
                {
                    allnum = orderQueryable.Sum(c => c.num),
                    allcostprice = orderQueryable.Sum(c => c.cost_price),
                    allmaprice = orderQueryable.Sum(c => c.market_price),
                    allcuprice = orderQueryable.Sum(c => c.custome_price),
                    tolallcostprice = orderQueryable.Sum(c => c.cost_price * c.num),
                    tolallcuprice = orderQueryable.Sum(c => c.custome_price * c.num),
                    tolallmaprice = orderQueryable.Sum(c => c.market_price * c.num),
                    avg = orderQueryable.Average(c => c.rate)
                };
            }
        }
        /// <summary>
        /// 导出文件
        /// </summary>
        /// <param name="trader_id"></param>
        /// <param name="bt"></param>
        /// <param name="et"></param>
        /// <param name="priceType">1成本价，2市场价，3客户价</param>
        /// <param name="typeId">0 excel ,1 pdf</param>
        public static void Export(int trader_id, DateTime bt, DateTime et, int priceType, int typeId, int dd_id = 0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var billList = db.V_OrderBill.Where(c => c.is_del == null || c.is_del == false).Where(c => c.create_time >= bt && c.create_time <= et);
                if (trader_id > 0)
                {
                    billList = billList.Where(c => c.trader_id == trader_id);
                }
                if (dd_id > 0)
                {
                    billList = billList.Where(c => c.factory_name.Trim() == SqlFunctions.StringConvert(dd_id * 1.0, 10, 0).Trim());
                }
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("客户"));
                dt.Columns.Add(new DataColumn("裁缝厂"));
                dt.Columns.Add(new DataColumn("款号"));
                dt.Columns.Add(new DataColumn("面料品番"));
                dt.Columns.Add(new DataColumn("款式"));
                dt.Columns.Add(new DataColumn("数量"));
                if (priceType != 0)
                    dt.Columns.Add(new DataColumn("单价"));
                dt.Columns.Add(new DataColumn("面料下单日"));
                dt.Columns.Add(new DataColumn("面料交期"));
                dt.Columns.Add(new DataColumn("送检日"));
                foreach (var r in billList.OrderByDescending(c => c.create_time).ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["客户"] = r.trader_name;
                    dr["裁缝厂"] = r.factory_fill_name;
                    dr["款号"] = r.clothing_number;
                    dr["面料品番"] = r.no;
                    dr["款式"] = r.styles;
                    dr["数量"] = r.num;
                    if (priceType == 1)
                    {
                        dr["单价"] = r.cost_price.ToDecimal("n2");
                    }
                    else if (priceType == 2)
                    {
                        dr["单价"] = r.market_price.ToDecimal("n2"); ;
                    }
                    else if (priceType == 3)
                    {
                        dr["单价"] = r.custome_price.ToDecimal("n2"); ;
                    }
                    dr["面料下单日"] = r.order_date.ToDateStr("yyyy/MM/dd");
                    dr["面料交期"] = r.get_date.ToDateStr("yyyy/MM/dd");
                    dr["送检日"] = r.submission_date.ToDateStr("yyyy/MM/dd");
                    dt.Rows.Add(dr);

                }
                if (typeId == 0)
                {
                    COM.Utility.AsposeExcel.MergeCellExport(dt, "定单.xls", 0);
                }
                else
                {
                    var pdf = COM.Utility.AsposeExcel.MergeCellSavePdf(dt, "定单.pdf", 0);
                    FileHelper.ShowPDF(pdf);
                }

            }

        }

        /// 保存报价单
        /// </summary>
        /// <param name="traderId">客户id</param>
        /// <param name="from">开始时间</param>
        /// <param name="to">结速时间，</param>
        /// <param name="priceType">价钱类型  1成本价 2市场价 3客户价</param>
        public static string SaveExport(int trader_id, DateTime bt, DateTime et, int priceType, int typeId, int dd_id = 0)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var billList = db.V_OrderBill.Where(c => c.is_del == null || c.is_del == false).Where(c => c.create_time >= bt && c.create_time <= et);
                if (trader_id > 0)
                {
                    billList = billList.Where(c => c.trader_id == trader_id);
                }
                if (dd_id > 0)
                {
                    billList = billList.Where(c => c.factory_name.Trim() == SqlFunctions.StringConvert(dd_id * 1.0, 10, 0).Trim());
                }
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("客户"));
                dt.Columns.Add(new DataColumn("裁缝厂"));
                dt.Columns.Add(new DataColumn("款号"));
                dt.Columns.Add(new DataColumn("面料品番"));
                dt.Columns.Add(new DataColumn("款式"));
                dt.Columns.Add(new DataColumn("数量"));
                if (priceType != 0)
                    dt.Columns.Add(new DataColumn("单价"));
                dt.Columns.Add(new DataColumn("面料下单日"));
                dt.Columns.Add(new DataColumn("面料交期"));
                dt.Columns.Add(new DataColumn("送检日"));
                foreach (var r in billList.OrderByDescending(c => c.create_time).ToList())
                {
                    DataRow dr = dt.NewRow();
                    dr["客户"] = r.trader_name;
                    dr["裁缝厂"] = r.factory_fill_name;
                    dr["款号"] = r.clothing_number;
                    dr["面料品番"] = r.no;
                    dr["款式"] = r.styles;
                    dr["数量"] = r.num;
                    if (priceType == 1)
                    {
                        dr["单价"] = r.cost_price.ToDecimal("n2");
                    }
                    else if (priceType == 2)
                    {
                        dr["单价"] = r.market_price.ToDecimal("n2"); ;
                    }
                    else if (priceType == 3)
                    {
                        dr["单价"] = r.custome_price.ToDecimal("n2"); ;
                    }
                    dr["面料下单日"] = r.order_date.ToDateStr("yyyy/MM/dd");
                    dr["面料交期"] = r.get_date.ToDateStr("yyyy/MM/dd");
                    dr["送检日"] = r.submission_date.ToDateStr("yyyy/MM/dd");
                    dt.Rows.Add(dr);

                }
                if (typeId == 0)
                {
                    return COM.Utility.AsposeExcel.MergeCellSaveExcel(dt, "定单.xls", 0);
                }
                else
                {
                    return COM.Utility.AsposeExcel.MergeCellSavePdf(dt, "定单.pdf", 0);
                }

            }

        }

        /// <summary>
        /// 创建裁剪单
        /// </summary>
        /// <param name="orderId"></param>
        public static void CreateCutBill(int orderId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var orderDeatil = db.FabricOrderBill.Where(c => c.order_id == orderId).Where(c=>c.is_del==null||c.is_del==false);
                var order = db.OrderBill.Single(c => c.o_id == orderId);
                order.is_cut = true;
                var removeOldCutList = db.CutBill.Where(c => c.order_id == order.o_id);
                foreach (var r in removeOldCutList)
                {
                    db.CutBill.DeleteObject(r);
                    foreach (var rr in db.CutBillShipment.Where(cbs => cbs.c_id == r.c_id))
                    {
                        db.CutBillShipment.DeleteObject(rr);
                    }
                }
                foreach (var r in orderDeatil)
                {
                    CutBill c = new CutBill()
                    {
                        order_id = r.order_id,
                        fd_id = r.fob_id,
                        color_name = r.color_name,
                        color_foreign = r.color_foreign,
                        trader_id = order.trader_id,
                        no = order.no,
                        clothing_number = order.clothing_number,
                        create_time = DateTime.Now,
                        creator_id = UserManager.GetCurrentUserInfo.user_id,
                        creator_name = UserManager.GetCurrentUserInfo.userName
                    };
                    db.CutBill.AddObject(c);

                }
                db.SaveChanges();
            }

        }
    }
}
