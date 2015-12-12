using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
namespace NFD.BLL
{
    //产样衣
    public class SampleManager
    {
        /// <summary>
        /// 保存样衣
        /// </summary>
        /// <param name="sa"></param>
        public static void Save(SampleDetail sa)
        {
            using (NFDEntities db = new NFDEntities())
            {
                //添加
                if (sa.samd_id== 0||sa.samd_id==null)
                {
                    sa.create_time = DateTime.Now;
                    sa.creator_id = UserManager.GetCurrentUserInfo.user_id;
                    sa.creator_name = UserManager.GetCurrentUserInfo.userName;
                    db.Insert<SampleDetail>(sa);
                }
                else
                {
                    //产样衣的面料米数量添加（放样成品数则减少）
                    FabricChanceEvent(sa, db);

                    //编辑
                    db.Update<SampleDetail>(sa.samd_id, new
                   {
                       modified_time = DateTime.Now,
                       paper_type = sa.paper_type,
                       no = sa.no,
                       necessary_number = sa.necessary_number,
                       hope_date = sa.hope_date,
                       factory_date = sa.factory_date,
                       actual_date = sa.actual_date,
                       indication_date = sa.indication_date,
                       remark = sa.remark,
                       bill_code = sa.bill_code,
                       fabric_num = sa.fabric_num,

                   });
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 产样衣的面料米数量添加（放样成品数则减少）
        /// </summary>
        /// <param name="sa"></param>
        /// <param name="db"></param>
        private static void FabricChanceEvent(SampleDetail sa, NFDEntities db)
        {
            var oldSmpleDatail = db.SampleDetail.Single(c => c.samd_id == sa.samd_id);
            var htd = db.AmplifierSample.Single(c => c.htd_id == oldSmpleDatail.htd_id); //大样明细

            var differenceVal = sa.fabric_num.ToInt() - oldSmpleDatail.fabric_num.ToInt();
            htd.finished_num = htd.finished_num.ToInt() - differenceVal.ToInt();
        }



        /// <summary>
        /// 获取样衣主表
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<SampleDetail> GetList(NFDEntities db)
        {
            return db.SampleDetail.Where(c => c.is_del == false || c.is_del == null);
        }
        /// <summary>
        /// 获取样衣明细表
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<V_Sample> GetVList(NFDEntities db)
        {
            return db.V_Sample.Where(c => c.is_del == false || c.is_del == null);
        }

        public static IQueryable<V_Sample_Detail> GetVDetailList(NFDEntities db)
        {
            return db.V_Sample_Detail.Where(c => c.is_del == false || c.is_del == null);
        }
        /// <summary>
        /// 更新样衣字段
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
                    string sql = "update SampleDetail set {0}='{1}' where sam_id={2} ".ToFormat(filed, date, id);
                    db.BySql(sql);
                }
                else
                {
                    string sql = "update SampleDetail set {0}=null where sam_id={1} ".ToFormat(filed, id);
                    db.BySql(sql);
                }
            }
        }
        /// <summary>
        /// 更新样衣字段
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filed"></param>
        /// <param name="val"></param>

        public static void UpdateFieldByVarchar(int id, string filed, string val)
        {
            val = val.ToSqlFilter();
            using (NFDEntities db = new NFDEntities())
            {
                if (val != null)
                {
                    string sql = "update SampleDetail set {0}='{1}' where sam_id={2} ".ToFormat(filed, val, id);
                    db.BySql(sql);
                }
                else
                {
                    string sql = "update SampleDetail set {0}=null where sam_id={1} ".ToFormat(filed, id);
                    db.BySql(sql);
                }
            }
        }

        /// <summary>
        /// 更新手织样状态
        /// </summary>
        /// <param name="htId"></param>
        /// <param name="status"></param>
        public static void ChangeStatus(int htId, int status)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var data = db.HandmadeThings.Single(c => c.ht_id == htId);
                data.status = status;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// 获取样衣历史
        /// </summary>
        /// <param name="db"></param>
        /// <param name="samdId"></param>
        /// <returns></returns>
        public static IQueryable<V_Sample_Detail_History> GetSampleListBySamdId(NFDEntities db, int samdId)
        {
            return db.V_Sample_Detail_History;
        }
        /// <summary>
        /// 获取样衣明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static SampleDetail GetSampleDetailById(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.SampleDetail.Single(c => c.samd_id == id);
            }
        }

        /// <summary>
        /// 重新产样衣
        /// </summary>
        /// <param name="h"></param>
        public static void ReSample(SampleDetailHistory h)
        {
            using (NFDEntities db = new NFDEntities())
            {
                try
                {

                    h.create_time = DateTime.Now;
                    h.creator_name = UserManager.GetCurrentUserInfo.userName;


                    var htd = db.SampleDetail.Single(c => c.samd_id == h.samd_id);
               
                    //记记放大样明细历史
                    SampleDetailHistory newH = new SampleDetailHistory()
                    {
                        htd_id = htd.htd_id,
                        samd_id = htd.samd_id,
                        actual_date = htd.actual_date,
                        bill_code = htd.bill_code,
                        create_time = DateTime.Now,
                        creator_id = UserManager.GetCurrentUserInfo.user_id,
                        creator_name = UserManager.GetCurrentUserInfo.userName,
                        hope_date = htd.hope_date,
                        factory_date = htd.factory_date,
                        indication_date = htd.indication_date,
                        necessary_number = htd.necessary_number,
                        no = htd.no,
                        paper_type = htd.paper_type,
                        remark = htd.remark,
                         fabric_num=htd.fabric_num


                    };
                    db.SampleDetailHistory.AddObject(newH);


                    //更新放大样明细

                    htd.bill_code = h.bill_code;
                    htd.paper_type = h.paper_type;
                    htd.indication_date = h.indication_date;
                    htd.no = h.no;
                    htd.necessary_number = h.necessary_number;
                    htd.hope_date = h.hope_date;
                    htd.factory_date = h.factory_date;
                    htd.actual_date = h.actual_date;
                    htd.remark = h.remark;
                    htd.is_next = true;
                    htd.fabric_num = h.fabric_num;

                    FabricChanceEvent(htd, db);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static void DeleteSampleDetail(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                var obj = db.SampleDetail.Single(it => it.samd_id == id);
                db.SampleDetail.DeleteObject(obj);
                db.SaveChanges();
            }
        }
    }
}
