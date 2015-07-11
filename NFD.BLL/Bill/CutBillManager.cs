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
    /// 裁剪
    /// </summary>
    public class CutBillManager
    {
        /// <summary>
        /// 获取裁剪
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_CutBill> GetV_CutBill(NFDEntities db)
        {
            return db.V_CutBill;
        }

        /// <summary>
        /// 保存裁剪单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveCuiBillAll(CutBill bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (bill.c_id.IsZero())
                {//添加

                    bill.create_time = DateTime.Now;
                    bill.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    bill.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.CutBill.AddObject(bill);
                    var isSaved = db.SaveChanges() > 0;
                    return isSaved;
                }
                else
                {
                    //编辑
                    db.Update<CutBill>(bill.c_id, new

                    {
                        actual_num = bill.actual_num,
                        arrival_num = bill.arrival_num,
                        modified_time = bill.modified_time,
                        delivers_num = bill.delivers_num,
                        note_num = bill.note_num,
                        size = bill.size,
                        will_num = bill.will_num,
                        out_num = bill.out_num,
                        bnum=bill.bnum,
                        check_date=bill.check_date,
                        remark=bill.remark,
                        export_price=bill.export_price,
                        wellhead_price=bill.wellhead_price,
                        no=bill.no,
                        color_foreign=bill.color_foreign


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
                return db.FalseDelete<CutBill>(id);
            }

        }


        /// <summary>
        /// 保存裁剪送货
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveCutBillShipment(CutBillShipment bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var cutBill=db.CutBill.Single(c=>c.c_id==bill.c_id);
                if (bill.cs_id.IsZero())
                {//添加

                    db.CutBillShipment.AddObject(bill);
                   
                    var isSuccess= db.SaveChanges() > 0;
                    UpdateBnum(bill, db, cutBill);
                    return isSuccess;
                }
                else
                {
                    //编辑
                    db.Update<CutBillShipment>(bill.c_id, new

                    {
                        shipment_date = bill.shipment_date,
                        num = bill.num

                    });
                    UpdateBnum(bill, db, cutBill);
                    return true;
                }

            }
        }

        private static void UpdateBnum(CutBillShipment bill, NFDEntities db, CutBill cutBill)
        {
            db.Update<CutBill>(bill.c_id, new
            {

                bnum = ((db.CutBillShipment.Where(c => c.c_id == bill.c_id).Select(c => c.num).Sum()) - cutBill.delivers_num).ToMoney()
            });
        }

        /// <summary>
        /// 删除裁剪送货
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DelCutBillShipment(params int[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                
                  var sid=id.Single();
                  var bill = db.CutBillShipment.Single(c => c.cs_id == sid);
                  var cutBill = db.CutBill.Single(c => c.c_id == bill.c_id);
                  db.Delete<CutBillShipment>(id);
                  UpdateBnum(bill,db,cutBill);
                  return true;
            }

        }

        /// <summary>
        /// 获取裁剪送货
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<CutBillShipment> GetCutBillShipment(NFDEntities db)
        {
            return db.CutBillShipment;
        }
    }
}
