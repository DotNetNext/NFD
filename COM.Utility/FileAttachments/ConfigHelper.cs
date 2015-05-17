using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace COM.Utility
{
    /// <summary>     
    /// 1. 功能：配制文件操作类
    /// 2. 作者：kaixuan
    /// 3. 创建日期：2014-10-24
    /// 4. 最后修改日期：2014-10-24
    /// </summary>  
    public class ConfigHelper
    {
        /// <summary>
        /// 获取appstring value
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppString(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void InsertAppString(string path, string key, string value)
        {
            //打开配置文件

            Configuration config = WebConfigurationManager.OpenWebConfiguration(path);

            //获取appSettings节点

            AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");

            //在appSettings节点中添加元素

            appSection.Settings.Add(key, value);
            config.Save();
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void UpdateAppString(string path, string key, string value)
        {
            //打开配置文件

            Configuration config = WebConfigurationManager.OpenWebConfiguration(path);

            //获取appSettings节点

            AppSettingsSection appSection = (AppSettingsSection)config.GetSection("appSettings");

            //删除appSettings节点中的元素

            appSection.Settings.Remove(key);

            appSection.Settings[key].Value = value;

            config.Save();

        }
    }
}
