using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
namespace NFD.BLL.Bill
{
    /// <summary>
    /// 产品计划管理
    /// </summary>
    public class ProductionPlanManager
    {
        /// <summary>
        /// 获取裁剪
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_ProductionPlan> GetProductionPlanBillList(NFDEntities db, int orderId)
        {
            if (orderId == 0)
            {
                return db.V_ProductionPlan.Where(c => c.is_del == null || c.is_del == false);
            }
            else
                return db.V_ProductionPlan.Where(c => c.order_id == orderId).Where(c => c.is_del == null || c.is_del == false);
        }

        /// <summary>
        /// 保存裁剪单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveProductionPlanAll(ProductionPlan bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (bill.pp_id.IsZero())
                {//添加
                    var orderBill = db.OrderBill.Single(c => c.o_id == bill.order_id);
                    bill.no = orderBill.no;
                    bill.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    bill.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.ProductionPlan.AddObject(bill);
                    var isSaved = db.SaveChanges() > 0;
                    return isSaved;
                }
                else
                {
                    //编辑
                    db.Update<ProductionPlan>(bill.pp_id, new  

                    {
                        rely_date = bill.rely_date,
                        no = bill.no,
                        num = bill.num,
                        send_date = bill.send_date,
                        send_check_date = bill.send_check_date,
                        ship_date = bill.ship_date,
                        style_name = bill.style_name,
                        style_no = bill.style_no,
                        factory_name = bill.factory_name,
                        a_get_date = bill.a_get_date,
                        c_end_date = bill.c_end_date,
                        c_report_date = bill.c_report_date,
                        c_start_date = bill.c_start_date,
                        clothing_number = bill.clothing_number,
                        confirm_date = bill.confirm_date,
                        f_get_date = bill.f_get_date,
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
                return db.FalseDelete<ProductionPlan>(id);
            }

        }

    }
}
