using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Common;
namespace NFD.BLL.Bill
{
    /// <summary>
    /// 报价单管理
    /// </summary>
    public class ChanceBillManager
    {
        /// <summary>
        /// 保存报价单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveChanceBill(ChanceBill bill)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                if (bill.bill_id == 0)
                {
                    //添加
                    bill.create_time = DateTime.Now;
                    bill.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    bill.creator_name = UserManager.GetCurrentUserInfo.userName;
                    if (bill.ht_id > 0)
                    {
                        var ht = db.HandmadeThings.Single(c => c.ht_id == bill.ht_id);
                        ht.status = PubEnum.HtStatus.报价.ToInt();
                    }
                    db.ChanceBill.AddObject(bill);
                }
                else
                {
                    db.Update<ChanceBill>(bill.bill_id, new
                    {
                        modified_time = DateTime.Now,
                        cost_price = bill.cost_price,
                        custome_price = bill.custome_price,
                        market_price = bill.market_price,
                        clothing_number = bill.clothing_number,
                        ht_specifications = bill.ht_specifications,
                        ht_no = bill.ht_no,
                        num = bill.num,
                        postage = bill.postage,
                        check_price=bill.check_price,
                        element=bill.element,
                        clothing_numbe3=bill.clothing_numbe3,
                        clothing_number2=bill.clothing_number2,
                        num2=bill.num2,
                        num3=bill.num3,
                        cost_price2=bill.cost_price2,
                        cost_price3=bill.cost_price3


                    });
                }
                return db.SaveChanges() > 0;
            }
        }
        public static ChanceBill GetChanceBillById(int id)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                var data = db.ChanceBill.Single(c => c.bill_id == id);
                return data;
            }
        }
        public static V_ChanceBill GetV_ChanceBillById(int id)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                var data = db.V_ChanceBill.Single(c => c.bill_id == id);
                return data;
            }
        }
        /// <summary>
        /// 获取报价单
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<ChanceBill> GetChanceList(NFDEntities db)
        {
            return db.ChanceBill.Where(c => c.is_del == false || c.is_del == null);
        }
        /// <summary>
        /// 获取报价单视图
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_ChanceBill> GetV_ChanceList(NFDEntities db)
        {
            return db.V_ChanceBill.Where(c => c.is_del == false || c.is_del == null);
        }
        /// <summary>
        /// 删除报价单
        /// </summary>
        /// <param name="id"></param>
        public static void DelBill(int id)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                db.FalseDelete<ChanceBill>(id);
            }
        }

        /// <summary>
        /// 保存报价单明细
        /// </summary>
        /// <param name="billDetail"></param>
        /// <returns></returns>
        public static bool SaveChanceBillDetail(ChanceBillDetail billDetail)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                if (billDetail.billd_id == 0)
                {
                    //添加
                    billDetail.create_time = DateTime.Now;
                    billDetail.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    billDetail.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.ChanceBillDetail.AddObject(billDetail);
                }
                else
                {
                    db.Update<ChanceBillDetail>(billDetail.billd_id, new
                    {
                        modified_time = DateTime.Now,
                        cost_price = billDetail.cost_price,
                        custome_price = billDetail.custome_price,
                        market_price = billDetail.market_price,
                        name = billDetail.name,
                        num = billDetail.num,
                        specifications=billDetail.specifications,
                        supplier_name=billDetail.supplier_name,


                    });
                }
                return db.SaveChanges() > 0;
            }
        }
        /// <summary>
        /// 获取明细
        /// </summary>
        /// <param name="billId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<ChanceBillDetail> GetChanceBillDetailListByBillId(int billId, Entities.Data.NFDEntities db)
        {
            var reval = db.ChanceBillDetail.Where(c => (c.is_del == null || c.is_del == false) && c.bill_id == billId);
            return reval;
        }
        /// <summary>
        /// 删除报价单
        /// </summary>
        /// <param name="billdId"></param>
        public static void DeleteChanceBillDetail(int billdId)
        {
            using (NFDEntities db = new Entities.Data.NFDEntities())
            {
                db.FalseDelete<ChanceBillDetail>(billdId);
            }
        }

      
    }
}
