using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Extension;
using NFD.Entities.Common;
using NFD.Entities.Data;
using COM.Utility;
namespace NFD.BLL.Common
{
    /// <summary>
    /// 附件管理
    /// </summary>
    public class AttaManager
    {
        public static void Save(NFDEntities db, List<Attas> attas)
        {
            foreach (var r in attas)
            {
                db.Attas.AddObject(r);
            }
        }

        /// <summary>
        /// 获取附件
        /// </summary>
        /// <returns></returns>
        public static List<Attas> GetAttasByHt_d(int htId, PubEnum.AttaType type)
        {
            using (NFDEntities db = new NFDEntities())
            {
                int typeId = type.ToInt();
                var list = db.Attas.Where(c => c.hid == htId && c.type_id == typeId).ToList();
                return list;
            }
        }
    }
}
