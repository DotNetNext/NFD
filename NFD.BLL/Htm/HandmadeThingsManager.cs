using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Extension;
using COM.Utility;
using System.Transactions;
using NFD.BLL.Common;
using NFD.Entities.Common;
namespace NFD.BLL
{
    /// <summary>
    /// 手织样
    /// </summary>
    public class HandmadeThingsManager
    {
        #region  基础操作
        public static void Del(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<HandmadeThings>(id);
            }

        }
        public static void Del(string[] id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                db.FalseDelete<HandmadeThings>(id);
            }

        }
        public static IQueryable<HandmadeThings> GetList(NFDEntities db)
        {
            return db.HandmadeThings.Where(c => c.is_del == false || c.is_del == null);
        }
        public static List<HandmadeThings> GetList()
        {
            using (NFDEntities db = new NFDEntities())
            {
                return GetList(db).ToList();
            }

        }
        public static HandmadeThings GetSingle(int id)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return GetList(db).Single(c => c.ht_id == id);
            }

        }
        public static HandmadeThings Save(HandmadeThings ht, string[] filePath)
        {
            var files = new List<Attas>();
            using (NFDEntities db = new NFDEntities())
            {
                string newPath = "/Content/File/ht/";
                db.Connection.Open();
                using (var tran = db.Connection.BeginTransaction())
                {
                    try
                    {

                        if (ht.ht_id == 0)
                        {
                            ht.create_time = DateTime.Now;
                            ht.creator_id = UserManager.GetCurrentUserInfo.user_id;
                            ht.creator_name = UserManager.GetCurrentUserInfo.userName;
                            db.Insert<HandmadeThings>(ht);
                            db.SaveChanges();

                        }
                        else
                        {
                            var delAttas = db.Attas.Where(c => c.hid == ht.ht_id);
                            foreach (var r in delAttas)
                            {
                                db.Attas.DeleteObject(r);
                            }

                            ht.create_time = DateTime.Now;
                            ht.creator_id = UserManager.GetCurrentUserInfo.user_id;
                            ht.creator_name = UserManager.GetCurrentUserInfo.userName;
                            db.Update<HandmadeThings>(ht.ht_id, new
                            {
                                modified_time = DateTime.Now,
                                modifier_id = UserManager.GetCurrentUserInfo.user_id,
                                modifier_name = UserManager.GetCurrentUserInfo.userName,
                                pro_no = ht.pro_no,
                                production_place = ht.production_place,
                                specifications = ht.specifications,
                                trader_id = ht.trader_id
                            });
                        }
                        if (filePath.IsHasValue())
                        {
                            foreach (var oledWebPath in filePath)
                            {
                                string oldPhysicsPath = FileHelper.GetMapPath(oledWebPath);
                                string fileName = FileHelper.GetFileName(oledWebPath);
                                string newPhysicsPath = FileHelper.GetMapPath(newPath) + ht.ht_id + @"\" + fileName;
                                string newWebPath = newPath + ht.ht_id + "/" + fileName;
                                FileHelper.Move(oldPhysicsPath, newPhysicsPath);
                                Attas a = new Attas()
                                {
                                    type_id = PubEnum.AttaType.手织样.ToInt(),
                                    add_time = DateTime.Now,
                                    filePath = newWebPath,
                                    hid = ht.ht_id,
                                    fileName=fileName
                                };
                                files.Add(a);
                            }
                        }
                        AttaManager.Save(db, files);
                        db.SaveChanges();
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {

                        tran.Rollback();
                        throw new Exception("保存手织样出错," + ex.Message);
                    }
                    return db.HandmadeThings.Single(c => c.ht_id == ht.ht_id);
                }
            }
        }
        #endregion


        /// <summary>
        /// 放大样
        /// </summary>
        /// <param name="id"></param>
        public static void AmplifierSample(int id)
        {

            using (NFDEntities db = new NFDEntities())
            {
                var ht = db.HandmadeThings.Single(c => c.ht_id == id);
                ht.status = 1;
                var htds = HandmadeThingsDetailManager.GetList(db).Where(c => c.ht_id == id);
                foreach (var r in htds)
                {
                    AmplifierSample am = new AmplifierSample()
                    {
                        ht_id = id,
                        htd_id = r.htd_id,
                        create_time = DateTime.Now,
                        creator_name = UserManager.GetCurrentUserInfo.userName,
                        remark=r.confirm_content,
                        creator_id = UserManager.GetCurrentUserInfo.user_id

                    };
                    db.AmplifierSample.AddObject(am);

                }
                db.SaveChanges();

            }
        }

        public static IQueryable<V_Ht_Htd_A_S_Sd> GetV_Ht_Htd_A_S_Sd(NFDEntities db)
        {
            return db.V_Ht_Htd_A_S_Sd;
        }
    }
}
