using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public class CongifHelper
    {
        /// <summary>
        /// 尝试获取config 文件配置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultValud">默认值</param>
        /// <returns></returns>
        public static string TryGetSettingsValue(string key, string defaultValud)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key","键不能为空或null！");
            }
            var value = ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(value))
            {
                return defaultValud;
            }
            return value;
        }
    }
}
