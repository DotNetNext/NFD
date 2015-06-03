using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data;
using System.Collections;
using System.Web.Routing;
using COM.Extension;
using System.Data.SqlClient;
namespace COM.Extension
{
    public static class ExtEF
    {
        private static Dictionary<string, string> _primaryKeys = null;
        /// <summary>
        /// 将匿名对象转成 SqlParameter[] 
        /// </summary>
        /// <param name="o">如 var o=new {id=1,name="张三"}</param>
        /// <returns>SqlParameter[]</returns>
        private static SqlParameter[] RouteToPars(RouteValueDictionary rvd)
        {
            List<SqlParameter> pars = new List<SqlParameter>();
            foreach (var r in rvd)
            {
                SqlParameter par;
                if (r.Value == null)
                {
                    par = new SqlParameter("@" + r.Key, DBNull.Value);
                }
                else
                {
                    par = new SqlParameter("@" + r.Key, r.Value);
                }
                pars.Add(par);
            }
            return pars.ToArray();
        }
        /// <summary>
        /// 获取主键
        /// </summary>
        /// <param name="type"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetPrimaryKey(Type type, string key)
        {
            if (_primaryKeys == null || !_primaryKeys.ContainsKey(key))
            {//无缓存

                string pk = "";
                //主键
                foreach (var prop in type.GetProperties())
                {
                    var attr = prop.GetCustomAttributes(typeof(System.Data.Objects.DataClasses.EdmScalarPropertyAttribute), false).FirstOrDefault() as System.Data.Objects.DataClasses.EdmScalarPropertyAttribute;
                    if (attr != null && attr.EntityKeyProperty)
                    {
                        pk = prop.Name;
                        break;
                    }
                }
                if (_primaryKeys == null)
                {
                    _primaryKeys = new Dictionary<string, string>();
                }
                _primaryKeys.Add(key, pk);
                return pk;
            }
            else
            {
                //有缓存
                return (_primaryKeys[key]);
            }

        }


        /// <summary>
        /// 批量删除 调用  
        /// BulkDelete<T>(new int[]{1,2,3})
        /// 或者
        /// BulkDelete<T>(3)
        /// </summary>
        /// <param name="oc"></param>
        /// <param name="whereIn">in的集合</param>
        public static bool Delete<TEntity>(this ObjectContext objectContext, params object[] whereIn)
        {
            try
            {
                Type type = typeof(TEntity);
                string key = type.FullName;
                bool isSuccess = false;
                if (whereIn != null && whereIn.Length > 0)
                {
                    string sql = "delete from {0} where {1} in ({2})".ToFormat(type.Name, GetPrimaryKey(type, key), whereIn.ToJoinSqlInVal());
                    isSuccess = objectContext.ExecuteStoreCommand(sql) > 0;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 批量删除 调用  将列is_Del设为1
        /// BulkDelete<T>(new int[]{1,2,3})
        /// 或者
        /// BulkDelete<T>(3)
        /// </summary>
        /// <param name="oc"></param>
        /// <param name="whereIn">in的集合</param>
        public static bool FalseDelete<TEntity>(this ObjectContext objectContext, params int[] whereIn)
        {
            try
            {
                Type type = typeof(TEntity);
                string key = type.FullName;
                bool isSuccess = false;
                if (whereIn != null && whereIn.Length > 0)
                {
                    string sql = "update  {0} set is_Del=1 where {1} in ({2})".ToFormat(type.Name, GetPrimaryKey(type, key), whereIn.Select(c=>c.ToString()).ToArray().ToJoinSqlInVal());
                    isSuccess = objectContext.ExecuteStoreCommand(sql) > 0;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 批量删除 调用  将列is_Del设为1
        /// BulkDelete<T>(new int[]{1,2,3})
        /// 或者
        /// BulkDelete<T>(3)
        /// </summary>
        /// <param name="oc"></param>
        /// <param name="whereIn">in的集合</param>
        public static bool FalseDelete<TEntity>(this ObjectContext objectContext, params object[] whereIn)
        {
            try
            {
                Type type = typeof(TEntity);
                string key = type.FullName;
                bool isSuccess = false;
                if (whereIn != null && whereIn.Length > 0)
                {
                    string sql = "update  {0} set is_Del=1 where {1} in ({2})".ToFormat(type.Name, GetPrimaryKey(type, key), whereIn.ToJoinSqlInVal());
                    isSuccess = objectContext.ExecuteStoreCommand(sql) > 0;
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="TEntity">更新实体类型</typeparam>
        /// <param name="objectContext"></param>
        /// <param name="primaryKeyValue">主键</param>
        /// <param name="setobj">列新参数 格式: new {id=1,name="xx"}</param>
        public static void Update<TEntity>(this ObjectContext objectContext, object primaryKeyValue, object setobj) where TEntity : class
        {

            Type type = typeof(TEntity);
            string key = type.FullName;
            var primaryKeyName = GetPrimaryKey(type, key);
            string sql = " update {0} set ".ToFormat(type.Name);
            RouteValueDictionary rvd = new RouteValueDictionary(setobj);
            foreach (var r in rvd)
            {
                if (r.Key != "EntityState" && r.Key != "EntityKey")
                {
                    sql += " {0} =@{0}  ,".ToFormat(r.Key);
                }
            }
            sql = sql.TrimEnd(',');
            sql += " where {0}='{1}' ".ToFormat(primaryKeyName, primaryKeyValue);
            objectContext.ExecuteStoreCommand(sql, RouteToPars(rvd));
        }

        /// <summary>
        /// 插入
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="objectContext"></param>
        /// <param name="entity"></param>
        public static void Insert<TEntity>(this ObjectContext objectContext, TEntity entity) where TEntity : class
        {
            objectContext.CreateObjectSet<TEntity>().AddObject(entity);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="objectContext"></param>
        /// <param name="sql">SQL语句</param>
        /// <param name="pars">参数</param>
        /// <returns></returns>
        public static int BySql(this ObjectContext objectContext, string sql, params SqlParameter[] pars)
        {
            return objectContext.ExecuteStoreCommand(sql);
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="objectContext">语句</param>
        /// <param name="sql">SQL</param>
        /// <param name="pars">参数</param>
        /// <returns>ObjectResult<TEntity> </returns>
        public static ObjectResult<TEntity> BySql<TEntity>(this ObjectContext objectContext, string sql, params SqlParameter[] pars)
        {
            return objectContext.ExecuteStoreQuery<TEntity>(sql);
        }
    }
}