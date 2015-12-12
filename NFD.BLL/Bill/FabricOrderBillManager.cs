using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
namespace NFD.BLL.Bill
{
    /// <summary>
    /// 面料订购单
    /// </summary>
    public class FabricOrderBillManager
    {
        /// <summary>
        /// 获取裁剪
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_FabricOrderBill> GetFabricOrderBillList(NFDEntities db, int orderId)
        {
            if (orderId == 0)
            {
                return db.V_FabricOrderBill.Where(c => c.is_del == null || c.is_del == false);
            }
            else
                return db.V_FabricOrderBill.Where(c => c.order_id == orderId).Where(c => c.is_del == null || c.is_del == false);
        }

        /// <summary>
        /// 获取裁剪
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_FabricOrderBill> GetFabricOrderBillList(NFDEntities db)
        {

            return db.V_FabricOrderBill;
        }

        /// <summary>
        /// 保存裁剪单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveFabricOrderBillAll(FabricOrderBill bill)
        {
            if (!bill.area.ToConvertString().Contains("±") || bill.area.ToConvertString().Contains("+")|| bill.area.ToConvertString().Contains("-"))
            {
                bill.area= "±"+bill.area;
            }
            using (NFDEntities db = new NFDEntities())
            {
                if (bill.fob_id.IsZero())
                {//添加
                    var orderBill = db.OrderBill.Single(c => c.o_id == bill.order_id);
                    bill.no = orderBill.no;
                    bill.trader_id = orderBill.trader_id;
                    bill.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    bill.creator_name = UserManager.GetCurrentUserInfo.userName;
                    bill.create_time = DateTime.Now;
                    db.FabricOrderBill.AddObject(bill);


                    FabricDetail df = new FabricDetail()
                    {
                      create_time=DateTime.Now,
                      creator_name=bill.creator_name,
                      color_name = bill.color_foreign,
                      price=bill.price,
                      order_quantity=bill.num,
                      order_id=bill.order_id,
                      color_foreign=bill.no,
                       mf=Convert.ToDecimal(bill.sdf.ToMoney()),
                      get_date=bill.get_date

                    };
                    db.FabricDetail.AddObject(df);

                    var isSaved = db.SaveChanges() > 0;
                    return isSaved;
                }
                else
                {
                    var fbBill = db.FabricOrderBill.Single(c => c.fob_id == bill.fob_id);
                    var orderBill = db.OrderBill.Single(c => c.o_id == fbBill.order_id);
                    //编辑
                    db.Update<FabricOrderBill>(bill.fob_id, new

                    {
                        datum = bill.datum,
                        get_date = bill.get_date,
                        no = bill.no,
                        num = bill.num,
                        price = bill.price,
                        element = bill.element,
                        trader_id = orderBill.trader_id,
                        clothing_number = bill.clothing_number,
                        address = bill.address,
                        area = bill.area,
                        color_foreign = bill.color_foreign,
                        color_name = bill.color_name,
                        specifications = bill.specifications,
                        supplier_id = bill.supplier_id,
                        sdf = bill.sdf,
                        remark = bill.remark



                    });

                    return true;
                }

            }
        }

        /// <summary>
        /// 删除裁剪单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelBill(params int[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.FalseDelete<FabricOrderBill>(id);
            }

        }



    }
}
