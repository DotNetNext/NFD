using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
namespace NFD.BLL.Bill
{
    /// <summary>
    /// 面料信息
    /// </summary>
    public class FabricDetailManager
    {
        /// <summary>
        /// 获取面料信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<FabricDetail> GetFabricDetailByOrderId(int orderId, NFDEntities db)
        {
            var reval = db.FabricDetail.Where(c => (c.is_del == null || c.is_del == false) && c.order_id == orderId);
            return reval;
        }
        /// <summary>
        /// 获取面料信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_FabricDetail> GetV_FabricDetailByOrderId(NFDEntities db)
        {
            var reval = db.V_FabricDetail;
            return reval;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static bool Save(FabricDetail fd)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (fd.fd_id.IsZero())
                {
                    fd.create_time = DateTime.Now;
                    fd.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    fd.creator_name = UserManager.GetCurrentUserInfo.userName;
                    fd.fabric_add_reduction = fd.fabric_arrival.ToDecimal() - fd.order_quantity.ToDecimal();
                    db.FabricDetail.AddObject(fd);
                    return db.SaveChanges() > 0;
                }
                else
                {
                    if (fd.fabric_add_reduction.IsNullOrEmpty()) { 
                      fd.fabric_add_reduction = fd.fabric_arrival.ToDecimal() - fd.order_quantity.ToDecimal();
                    }
                    db.Update<FabricDetail>(fd.fd_id, new 
                    {
                        clothes_orders_num = fd.clothes_orders_num,
                        color_foreign=fd.color_foreign,
                        color_name = fd.color_name,
                        consumption = fd.consumption,
                        fabric_add_reduction = fd.fabric_add_reduction,
                        fabric_arrival = fd.fabric_arrival,
                        get_date = fd.get_date,
                        mf = fd.mf,
                        modified_time = DateTime.Now,
                        order_quantity = fd.order_quantity,
                        subtotal = fd.subtotal,
                        price=fd.price

                    });
                    
                    return true;
                }
            }
        }

        public static bool Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
              return  db.FalseDelete<FabricDetail>(id);
            }
        }
    }
}
