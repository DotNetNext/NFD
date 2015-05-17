//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using NFD.Entities.Data;
//using COM.Extension;
//namespace NFD.BLL
//{
//    public class HomeManager
//    {
//        /// <summary>
//        /// 获取列表
//        /// </summary>
//        /// <param name="total">总数</param>
//        /// <param name="pageSize">页码</param>
//        /// <param name="pageIndex">第几页</param>
//        /// <returns></returns>
//        public static List<Home> GetHomeList(ref int total, string name, int pageSize, int pageIndex)
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                var dbList = db.Home.Where(c => string.IsNullOrEmpty(name) || c.Name.Contains(name));
//                total = dbList.Count();
//                return dbList.OrderByDescending(c => c.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

//            }
//        }

//        /// <summary>
//        /// 根据ID查单条
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static Home GetHomeById(int id)
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                return db.Home.Single(c => c.Id == id);
//            }
//        }

//        /// <summary>
//        /// 保存
//        /// </summary>
//        /// <param name="h"></param>
//        public static bool Save(Home h)
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                //插入实体
//                Home newhome = new Home()
//                {
//                    Content = "我是新人",
//                    Name = "新人"
//                };

                 
//                /**********************EF升级版性***********************/
//                //优点： 性能和原生ADO.NET媲美，数据库负担比ADO.NET更少，代码调用简单 
//                //性能优化：减少SQL执行计划，添加实体缓存 ，删除、更新无需先查后后处理
//                //引用 COM.Utility和COM.Extension
//                //调用如下

//                //查询部分用法和以前一样
//                db.Home.ToList();

//                //插入
//                db.Insert<Home>(newhome);
//                //修改
//                db.Update<Home>(newhome.Id /*主键*/, new { name = "元老", content = "修改内容" });
//                //删除一条
//                db.Delete<Home>(newhome.Id);
//                //删除多条
//                db.Delete<Home>(1, 2, 3);
//                db.Delete<Home>(new string[] { "1", "2" });
 
//                //获用SQL获取影响行数
//                db.BySql("delete home where id=1");//可以参数化重载
//                //获用SQL返回实体
//                db.BySql<Home>("select * from home").ToList();

//            }

//            using (TestEntities db = new TestEntities())
//            {
//                if (h.Id == 0)
//                {
//                    //添加
//                    db.Insert<Home>(h);
//                }
//                else
//                {
//                    //修改
//                    db.Update<Home>(h.Id, new { name = h.Name, content = h.Content });

//                }
//                return db.SaveChanges() > 0;
//            }
//        }

//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static bool Delete(int id)
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                return db.Delete<Home>(id);//可以删除多条 db.Delete<Home>(数组)
//            }
//        }
//        /// <summary>
//        /// 删除
//        /// </summary>
//        /// <param name="id"></param>
//        /// <returns></returns>
//        public static bool Delete(string [] id)
//        {
//            using (TestEntities db = new TestEntities())
//            {
//                return db.Delete<Home>(id);//可以删除多条 db.Delete<Home>(数组)
//            }
//        }
//        /// <summary>
//        /// 获home对象
//        /// </summary>
//        /// <param name="db"></param>
//        /// <returns></returns>
//        public static IQueryable<Home> GetHome(TestEntities db)
//        {
//            return db.Home;
//        }
//    }
//}
