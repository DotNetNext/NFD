using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Utility;
using COM.Extension;
using NFD.Entities.Data;
namespace NFD.BLL
{
    /// <summary>
    /// 客户
    /// </summary>
    public class TraderManager
    {
        public static void Save(Trader trader)
        {
            using (NFDEntities db = new NFDEntities())
            {
                //添加
                if (trader.trader_id == 0)
                {
                    trader.create_time = DateTime.Now;
                    trader.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    trader.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.Insert<Trader>(trader);
                }
                else
                {
                    //编辑
                    db.Update<Trader>(trader.trader_id, new
                    {
                        url = trader.url,
                        code = trader.code,
                        name = trader.name,
                        modified_time = DateTime.Now,
                        fax_number = trader.fax_number,
                        mobile = trader.mobile,
                        lev = trader.lev,
                        modifier_id = UserManager.GetCurrentUserInfo.user_id,
                        modifier_name = UserManager.GetCurrentUserInfo.userName,
                        email = trader.email,
                        qq=  trader.qq,
                        tel=trader.tel,
                        remark=trader.remark

                    });
                }
                db.SaveChanges();
            }
        }

        public static void Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<Trader>(id);
            }

        }
        public static void Del(string[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<Trader>(id);
            }

        }


        public static IQueryable<Trader> GetList(NFDEntities db)
        {
            return db.Trader.Where(c => c.is_del == false || c.is_del == null);
        }


        public static List<Trader> GetList()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return GetList(db).ToList();
            }

        }


    }
}
