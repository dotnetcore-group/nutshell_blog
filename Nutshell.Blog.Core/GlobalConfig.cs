using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public static class GlobalConfig
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public static int PageSize
        {
            get
            {
                return int.Parse(CongifHelper.TryGetSettingsValue("pageSize", "10"));
            }
        }

        /// <summary>
        /// 域名
        /// </summary>
        public static string UrlPrefix
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("urlPrefix", "http://localhost/");
            }
        }

        /// <summary>
        /// memcache 主机地址
        /// </summary>
        public static string MemcacheHost
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("memcachehost", "127.0.0.1:11211");
            }
        }

        /// <summary>
        /// lucene.net dir目录
        /// </summary>
        public static string LuceneDir
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("lucenedir", @"F:\lucenedir");
            }
        }


        #region 邮箱的配置
        /// <summary>
        /// 企业邮箱smtp主机
        /// </summary>
        public static string EmailHost
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("host", "smtp.mxhichina.com");
            }
        }

        /// <summary>
        /// 企业邮箱smtp端口
        /// </summary>
        public static string EmailPort
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("port", "25");
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        public static string EmailFrom
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("fromAddress", "admin@foxdotnet.top");
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public static string EmailFromPassword
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("fromPassword", "ZengAnde936423");
            }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public static string EmailFromName
        {
            get
            {
                return CongifHelper.TryGetSettingsValue("formName", "service");
            }
        } 
        #endregion
    }
}
