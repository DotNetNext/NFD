using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
namespace NFD.BLL.Bill
{
    public class AccessoriesDetailManager
    {
        /// <summary>
        /// 获取面料信息
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<AccessoriesDetail> GetAccessoriesDetailByOrderId(int orderId, NFDEntities db)
        {
            var reval = db.AccessoriesDetail.Where(c => (c.is_del == null || c.is_del == false) && c.order_id == orderId);
            return reval;
        }


        /// <summary>
        /// 获取面料信息(视图)
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_AccessoriesDetail> GetV_AccessoriesDetail(NFDEntities db)
        {
            var reval = db.V_AccessoriesDetail;
            return reval;
        }


        public static bool Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
               return  db.FalseDelete<AccessoriesDetail>(id);
            }
        }

        public static bool Save(AccessoriesDetail ad)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (ad.ad_id.IsZero())
                {
                    DictionariesDetail d = new DictionariesDetail();
                    if (ad.supplier_name.IsInt())
                    {
                        var sid = ad.supplier_name.ToInt();
                        if (db.DictionariesDetail.Any(c => c.dd_id == sid))
                        {
                            d = db.DictionariesDetail.Single(c => c.dd_id == sid);
                        }
                    }
                    ad.supplier = d.dd_id;
                    ad.supplier_name = d.d_name;
                    ad.create_time = DateTime.Now;
                    ad.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    ad.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.AccessoriesDetail.AddObject(ad);
                    return db.SaveChanges() > 0;
                }
                else
                {
                    DictionariesDetail d = new DictionariesDetail();
                    if (ad.supplier_name.IsInt())
                    {
                        var sid = ad.supplier_name.ToInt();
                        if (db.DictionariesDetail.Any(c => c.dd_id == sid))
                        {
                            d = db.DictionariesDetail.Single(c => c.dd_id == sid);
                        }
                    }
                    db.Update<AccessoriesDetail>(ad.ad_id, new
                    {

                        modified_time = DateTime.Now,
                        get_date = ad.get_date,
                        get_num = ad.get_num,
                        order_num = ad.order_num,
                        price = ad.price,
                        specifications = ad.specifications,
                        supplier = d.dd_id,
                        supplier_name = d.d_name,
                        surplus_num = ad.surplus_num,
                        unit = ad.unit,
                        use_num = ad.use_num,
                        ad_name = ad.ad_name

                    });
                    return true;
                }
            }

        }
    }
}
