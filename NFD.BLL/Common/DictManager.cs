using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using COM.Utility;
using NFD.Entities.Data;
namespace NFD.BLL
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class DictManager
    {
        /// <summary>
        /// 获取裁缝名称
        /// </summary>
        /// <returns></returns>
        public static List<DictionariesDetail> GetFactory()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.DictionariesDetail.Where(c => c.d_key == 1).ToList();
            }
        }

        /// <summary>
        /// 获取供应商
        /// </summary>
        /// <returns></returns>
        public static List<DictionariesDetail> GetProvider()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.DictionariesDetail.Where(c => c.d_key ==2).ToList();
            }
        }

        /// <summary>
        /// 获取字典分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Dictionaries GetDictionariesById(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var reval = db.Dictionaries.Single(c => c.key == id);
                return reval;
            }
        }

        public static List<Dictionaries> GetDictList()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.Dictionaries.ToList();
            }
        }

        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<DictionariesDetail> GetDictDetail(NFDEntities db)
        {
            return db.DictionariesDetail;
        }

        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public static bool Save(DictionariesDetail dd)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (dd.dd_id.IsZero())
                {
                    dd.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    dd.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.DictionariesDetail.AddObject(dd);
                    return db.SaveChanges() > 0;
                }
                else
                {
                    db.Update<DictionariesDetail>(dd.dd_id, new
                    {
                        d_key = dd.d_key,
                        d_name = dd.d_name,
                        sort = dd.sort,
                        value1 = dd.value1,
                        value2 = dd.value2,
                        value3 = dd.value3

                    });
                    return true;
                }
            }
        }

    }
}
