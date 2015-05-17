using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
namespace NFD.BLL
{
    public class AmplifierSampleManager
    {
        public static void Save(AmplifierSample afs)
        {
            using (NFDEntities db = new NFDEntities())
            {
                //添加
                if (afs.as_id == 0)
                {
                    //afs.create_time = DateTime.Now;
                    //afs.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    //afs.creator_name = UserManager.GetCurrentUserInfo.userName;
                    //db.Insert<AmplifierSample>(afs);
                }
                else
                {
                    //编辑
                    db.Update<AmplifierSample>(afs.as_id, new
                    {

                        Indicate_day = afs.Indicate_day,
                        factory_date = afs.factory_date,
                        actual_date = afs.actual_date,
                        factory_date2 = afs.factory_date2,
                        factory_num = afs.factory_num,
                        finished_num = afs.finished_num,
                        hope_data = afs.hope_data,
                        m_num = afs.m_num,
                        remark = afs.remark,
                        warehouse_num = afs.warehouse_num,
                        modified_time = DateTime.Now,
                        binfan=afs.binfan,
                       binname= afs.binname

                    });
                }
                db.SaveChanges();
            }
        }

        public static void Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<AmplifierSample>(id);
            }

        }
        public static void Del(string[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<AmplifierSample>(id);
            }

        }


        public static IQueryable<AmplifierSample> GetList(NFDEntities db)
        {
            return db.AmplifierSample.Where(c => c.is_del == false || c.is_del == null);
        }
        public static IQueryable<V_AmplifierSample> GetVList(NFDEntities db)
        {
            return db.V_AmplifierSample.Where(c => c.is_del == false || c.is_del == null);
        }

        public static List<AmplifierSample> GetList()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return GetList(db).ToList();
            }

        }


        public static void UpdateField(int id, string filed, DateTime? date)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (date != null)
                {
                    string sql = "update AmplifierSample set {0}='{1}' where ht_id={2} ".ToFormat(filed, date, id);
                    db.BySql(sql);
                }
                else
                {
                    string sql = "update AmplifierSample set {0}=null where ht_id={1} ".ToFormat(filed, id);
                    db.BySql(sql);
                }
            }
        }

        public static void Sample(int id, string sam_name, DateTime? indication_date, string paper_type)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.Connection.Open();
                using (var tran = db.Connection.BeginTransaction())
                {

                    var ht = db.HandmadeThings.Single(c => c.ht_id == id);
                    ht.status = 2;
                    var hts = db.HandmadeThingsDetail.Where(c => c.ht_id == id);
                    //不存在才创建
                    var isCreate = !db.Sample.Any(c => c.ht_id == id);
                    if (isCreate)
                    {
                        var sample = new Sample()
                        {
                            ht_id = id,
                            create_time = DateTime.Now,
                            creator_id = UserManager.GetCurrentUserInfo.user_id,
                            creator_name = UserManager.GetCurrentUserInfo.userName,
                            sam_name = sam_name
                        };
                   
                        db.Sample.AddObject(sample);
                        db.SaveChanges();
                        if (hts != null)
                        {
                            foreach (var r in hts)
                            {
                                var ase=db.AmplifierSample.Single(c=>c.htd_id==r.htd_id);
                                SampleDetail sd = new SampleDetail()
                                {
                                    creator_id = UserManager.GetCurrentUserInfo.user_id,
                                    creator_name = UserManager.GetCurrentUserInfo.userName,
                                    create_time = DateTime.Now,
                                    indication_date = indication_date,
                                    paper_type = paper_type,
                                    sam_id = sample.sam_id,
                                    htd_id = r.htd_id,
                                    fabric_num= ase.factory_num

                                };
                                db.SampleDetail.AddObject(sd);

                            }
                        }
                        db.SaveChanges();
                    }
                    tran.Commit();
                }
            }
        }

        /// <summary>
        /// 重新放大样
        /// </summary>
        /// <param name="h"></param>
        public static void ReAs(AmplifierSampleHistory h)
        {
            using (NFDEntities db = new NFDEntities())
            {
                try
                {

                    h.create_time = DateTime.Now;
                    h.creator_name = UserManager.GetCurrentUserInfo.userName;


                    var htd = db.AmplifierSample.Single(c => c.as_id == h.as_id);
                    //记记放大样明细历史
                    AmplifierSampleHistory newH = new AmplifierSampleHistory()
                    {
                        htd_id = htd.htd_id,
                        as_id = htd.as_id,
                        actual_date = htd.actual_date,
                        confirm_content = htd.confirm_content,
                        create_time = DateTime.Now,
                        creator_id = UserManager.GetCurrentUserInfo.user_id,
                        creator_name = UserManager.GetCurrentUserInfo.userName,
                        factory_date = htd.factory_date,
                        factory_date2 = htd.factory_date2,
                        factory_num = htd.factory_num,
                        finished_num = htd.finished_num,
                        hope_data = htd.hope_data,
                        Indicate_day = htd.Indicate_day,
                        warehouse_num = htd.warehouse_num,
                        m_num = htd.m_num,
                        remark = htd.remark


                    };
                    db.AmplifierSampleHistory.AddObject(newH);


                    //更新放大样明细
                    htd.remark = h.remark;
                    htd.Indicate_day = h.Indicate_day;
                    htd.hope_data = h.hope_data;
                    htd.factory_date = h.factory_date;
                    htd.actual_date = h.actual_date;
                    htd.m_num = h.m_num;
                    htd.finished_num = h.finished_num;
                    htd.factory_date2 = h.factory_date2;
                    htd.factory_num = h.factory_num;
                    htd.warehouse_num = h.warehouse_num;
                    htd.remark = h.remark;
                    htd.is_next = true;
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }


        /// <summary>
        /// 获取放大样明细
        /// </summary>
        /// <param name="asId"></param>
        /// <returns></returns>
        public static AmplifierSample GetAmplifierSampleById(int asId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.AmplifierSample.Single(c => c.as_id == asId);
            }
        }

        /// <summary>
        /// 获取大样历史
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_AmplifierSampleHistory> GetAmplifierSampleHistoryList(NFDEntities db)
        {
            return db.V_AmplifierSampleHistory;
        }
    }
}
