using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
namespace NFD.BLL
{
    /// <summary>
    /// 手织样明细
    /// </summary>
    public class HandmadeThingsDetailManager
    {
        public static void Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<HandmadeThingsDetail>(id);
                db.BySql("update AmplifierSample set is_del=1 where  htd_id= " + id);
            }

        }
        public static void Del(string[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<HandmadeThingsDetail>(id);
            }

        }


        public static IQueryable<HandmadeThingsDetail> GetList(NFDEntities db)
        {
            return db.HandmadeThingsDetail.Where(c => c.is_del == false || c.is_del == null);
        }


        public static List<HandmadeThingsDetail> GetList()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return GetList(db).ToList();
            }

        }

        public static HandmadeThingsDetail GetHtdById(int htdId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.HandmadeThingsDetail.Single(c => c.htd_id == htdId);
            }

        }

        public static void Save(HandmadeThingsDetail htd)
        {
            using (NFDEntities db = new NFDEntities())
            {
                //添加
                if (htd.htd_id == 0)
                {
                    htd.create_time = DateTime.Now;
                    htd.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    htd.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.Insert<HandmadeThingsDetail>(htd);
                }
                else
                {
                    //编辑
                    db.Update<HandmadeThingsDetail>(htd.htd_id, new
                    {
                        modified_time = DateTime.Now,
                        color_foreign = htd.color_foreign,
                        color_name = htd.color_name,
                        Indicate_day = htd.Indicate_day,
                        confirm_day = htd.confirm_day,
                        actual_sending_date = htd.actual_sending_date,
                        predetermined_sent_day = htd.predetermined_sent_day,
                        confirm_content=htd.confirm_content
                    });
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 统一更新列
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filed"></param>
        /// <param name="date"></param>
        public static void UpdateField(int id, string filed, DateTime? date)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (date != null)
                {
                    string sql = "update HandmadeThingsDetail set {0}='{1}' where ht_id={2} ".ToFormat(filed, date, id);
                    db.BySql(sql);
                }
                else
                {
                    string sql = "update HandmadeThingsDetail set {0}=null where ht_id={1} ".ToFormat(filed, id);
                    db.BySql(sql);
                }
            }
        }

        /// <summary>
        /// 重新打手织样
        /// </summary>
        /// <param name="htdId"></param>
        public static void ReHtm(HandmadeThingsDetailHistory h)
        {
            using (NFDEntities db = new NFDEntities())
            {
                try
                {
                    h.create_time = DateTime.Now;
                    h.creator_name = UserManager.GetCurrentUserInfo.userName;


                    var htd = db.HandmadeThingsDetail.Single(c => c.htd_id == h.htd_id);
                    //记录手织样明细历史
                    HandmadeThingsDetailHistory newH = new HandmadeThingsDetailHistory()
                    {
                        htd_id = htd.htd_id,
                        actual_sending_date = htd.actual_sending_date,
                        color_foreign = htd.color_foreign,
                        color_name = htd.color_name,
                        create_time = DateTime.Now,
                        confirm_day = htd.confirm_day,
                        confirm_content = htd.confirm_content,
                        creator_name = UserManager.GetCurrentUserInfo.userName,
                        creator_id = UserManager.GetCurrentUserInfo.user_id,
                        Indicate_day = htd.Indicate_day,
                        predetermined_sent_day = htd.predetermined_sent_day,
                        remark = htd.remark


                    };
                    db.HandmadeThingsDetailHistory.AddObject(newH);


                    //更新手织样明细
                    htd.remark = h.remark;
                    htd.color_foreign = h.color_foreign;
                    htd.color_name = h.color_name;
                    htd.Indicate_day = h.Indicate_day;
                    htd.is_next = true;
                    htd.predetermined_sent_day = h.predetermined_sent_day;
                    htd.actual_sending_date = h.actual_sending_date;
                    htd.confirm_day = h.confirm_day;
                    htd.confirm_content = h.confirm_content;


                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }
        /// <summary>
        /// 获取打样记录
        /// </summary>
        /// <param name="htdId"></param>
        /// <returns></returns>
        public static IQueryable<HandmadeThingsDetailHistory> GetHtdHistoryByHtdId(NFDEntities db, int htdId)
        {

            return db.HandmadeThingsDetailHistory.Where(c => c.htd_id == htdId).OrderBy(c => c.create_time);


        }
    }
}
