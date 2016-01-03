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
    public class CutBill2Manager
    {
        /// <summary>
        /// 获取裁剪
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<CutBill2> GetV_CutBill(NFDEntities db)
        {
            return db.CutBill2;
        }

        /// <summary>
        /// 保存裁剪单
        /// </summary>
        /// <param name="bill"></param>
        /// <returns></returns>
        public static bool SaveCuiBillAll(CutBill2 bill)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (bill.bt2_id.IsZero())
                {//添加
                    if (bill.size.IsHasValue() && bill.size.Contains(","))
                    {
                        var sizeArray = bill.size.Split(',');
                        int i = 0;
                        foreach (var size in sizeArray)
                        {
                            CutBill2 b = new CutBill2()
                            {
                                fabric_arrival = bill.fabric_arrival,
                                delivers_num2 = bill.delivers_num2,
                                size = size,
                                color_foreign = bill.color_foreign,
                                color_name = bill.color_name,
                                order_quantity = bill.order_quantity,
                                order_id=bill.order_id
                            };
                            try
                            {
                                var note_num = RequestHelper.QueryString("note_num").Split(',');
                                var will_num = RequestHelper.QueryString("will_num").Split(',');
                                var actual_num = RequestHelper.QueryString("actual_num").Split(',');
                                var check_num = RequestHelper.QueryString("check_num").Split(',');
                                var delivers_num = RequestHelper.QueryString("delivers_num").Split(',');
                                b.note_num = Convert.ToDecimal( note_num[i].ToDou());
                                b.will_num = Convert.ToDecimal(will_num[i].ToDou());
                                b.actual_num = Convert.ToDecimal(actual_num[i].ToDou());
                                b.check_num = Convert.ToDecimal(check_num[i].ToDou());
                                b.delivers_num = Convert.ToDecimal(delivers_num[i].ToDou());
                                db.CutBill2.AddObject(b);
                            }
                            catch (Exception)
                            {
                                
                               
                            }
                            ++i;
                        }
                        var isSaved = db.SaveChanges() > 0;
                        return isSaved;
                    }
                    else
                    {
                        db.CutBill2.AddObject(bill);
                        var isSaved = db.SaveChanges() > 0;
                        return isSaved;
                    }
                }
                else
                {
                    //编辑
                    db.Update<CutBill2>(bill.bt2_id, new

                    {
                        actual_num = bill.actual_num,
                        delivers_num = bill.delivers_num,
                        note_num = bill.note_num,
                        size = bill.size,
                        will_num = bill.will_num,
                        color_foreign = bill.color_foreign,
                        color_name = bill.color_name,
                        order_quantity = bill.order_quantity,
                        check_num = bill.check_num,
                        check_num2 = bill.check_num2,
                        will_num2 = bill.will_num2,
                        fabric_arrival = bill.fabric_arrival,
                        actual_num2 = bill.actual_num2,
                        delivers_num2 = bill.delivers_num2

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
                return db.Delete<CutBill2>(id);
            }

        }


    }
}
