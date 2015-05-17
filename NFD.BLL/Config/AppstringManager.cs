using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COM.Utility;
using COM.Extension;
namespace NFD.BLL
{
    public class AppstringManager
    {
        /// <summary>
        /// 系统名
        /// </summary>
        public static string GetSystemName = ConfigHelper.GetAppString("system_name");

        /// <summary>
        /// 默认grid pagesize
        /// </summary>
        public static int GetPageSize = ConfigHelper.GetAppString("grid_page_size").ToInt();


             /// <summary>
        /// 默认grid height
        /// </summary>
        public static int GetGridHeight = ConfigHelper.GetAppString("grid_height").ToInt();

        /// <summary>
        /// 默认grid windows height
        /// </summary>
        public static int GetGridWindowHeight = ConfigHelper.GetAppString("grid_window_height").ToInt();

        /// <summary>
        /// 获取邮箱用户名
        /// </summary>
        public static string GetMailUserName = ConfigHelper.GetAppString("mailUserName");

        /// <summary>
        /// 获取邮箱密码
        /// </summary>
        public static string GetMailPassword = ConfigHelper.GetAppString("mailPassword");

        /// <summary>
        /// 获取邮箱smtp服务器地址
        /// </summary>
        public static string GetMailSmtp = ConfigHelper.GetAppString("mailSmtp");

        /// <summary>
        /// 邮件显示名称
        /// </summary>
        public static string GetMailName = ConfigHelper.GetAppString("mailName");
    }
}
