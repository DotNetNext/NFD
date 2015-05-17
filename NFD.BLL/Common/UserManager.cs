using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NFD.Entities.Data;
using COM.Utility;
using NFD.Entities.Common;
using NFD.ExceptionHandling;
using COM.Extension;
namespace NFD.BLL
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManager
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="u">反回登录后的userInfo</param>
        /// <returns></returns>
        public static bool Login(ref UserInfo u)
        {

            using (NFDEntities db = new NFDEntities())
            {
                try
                {
                    string password = Encrypt.MD5Encrypt(u.password);
                    string userName = u.userName;
                    var user = db.UserInfo.Where(c => c.password == password && c.userName == userName).ToList();
                    var isLogin = user.Count == 1;
                    if (isLogin)
                    {
                        u = user.Single();
                        CookiesHelper.AddCookie(PubConst.COOKIES_LOGIN, u.ToJson());
                    }
                    return isLogin;
                }
                catch (Exception ex)
                {
                    throw ex;

                }
            }
        }

        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                return CookiesHelper.GetCookie(PubConst.COOKIES_LOGIN) != null;
            }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public static UserInfo GetCurrentUserInfo
        {
            get
            {
                try
                {
                    var reval = CookiesHelper.GetCookieValue(PubConst.COOKIES_LOGIN).ToModel<UserInfo>();
                    return reval;
                }
                catch (Exception ex)
                {
                    ErrorParams pars = new ErrorParams()
                    {
                        AppendMessage = "获取用户出错",
                        Code = ex.GetHashCode().ToString(),
                        Method = "NFD.BLL.UserManager.GetCurrentUserInfo"
                    };
                    throw new UserException(ex, pars);
                }
            }
        }

        /// <summary>
        /// 注销
        /// </summary>
        public static void Logout()
        {
            SessionHelper.Del(PubConst.COOKIES_LOGIN);
        }





        /// <summary>
        /// 获取所有字典
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public static IQueryable<UserInfo> GetUserList(NFDEntities db)
        {
            var list = db.UserInfo.ToList().Select(c => new UserInfo() { password = Encrypt.MD5Decrypt(c.password), userName = c.userName, user_id = c.user_id, add_time = c.add_time, role_id = c.role_id }).ToList();
            return list.AsQueryable();
        }

        /// <summary>
        /// 保存字典
        /// </summary>
        /// <param name="dd"></param>
        /// <returns></returns>
        public static bool Save(UserInfo dd)
        {
            using (NFDEntities db = new NFDEntities())
            {
                if (dd.user_id.IsZero())
                {
                    if (dd.password.IsHasValue())
                    {
                        dd.password = Encrypt.MD5Encrypt(dd.password);
                    }
                    dd.add_time = DateTime.Now;
                    db.UserInfo.AddObject(dd);
                    return db.SaveChanges() > 0;
                }
                else
                {
                    if (dd.password.IsHasValue())
                    {
                        dd.password = Encrypt.MD5Encrypt(dd.password);
                    }
                    db.Update<UserInfo>(dd.user_id, new
                    {
                        password = dd.password,
                        userName = dd.userName

                    });
                    return true;
                }
            }
        }

        /// <summary>
        /// 保存用户菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menus"></param>
        /// <returns></returns>
        public static void SaveUserMenuIds(int userId, string menus)
        {
            using (NFDEntities db = new NFDEntities())
            {

                string sql = "delete UserInfoMenuMapping where user_id={0}".ToFormat(userId);
                db.BySql(sql);
                var model = menus.ToModel<List<Ztree>>();
                if (model.IsHasValue())
                {
                    foreach (var r in model)
                    {
                        UserInfoMenuMapping um = new UserInfoMenuMapping()
                        {
                            menu_id = r.id,
                            user_id = userId
                        };
                        db.UserInfoMenuMapping.AddObject(um);
                    }
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 获取用户的菜单
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static List<UserInfoMenuMapping> GetUserInfoMenuByUserId(int userId)
        {
            using (NFDEntities db = new NFDEntities())
            {
                return db.UserInfoMenuMapping.Where(c=>c.user_id==userId).ToList();
            }
        }

    }
}
