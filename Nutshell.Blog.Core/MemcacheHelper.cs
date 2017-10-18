/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core
 * 文件名：MemcacheHelper
 * 版本号：V1.0.0.0
 * 唯一标识：1609aca4-2277-425b-82c5-58a9da5ef7a9
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 15:10:32
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 15:10:32
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public class MemcacheHelper
    {
        readonly static string[] services = ConfigurationManager.AppSettings["memcachehost"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        static readonly MemcachedClient mc = null;

        static MemcacheHelper()
        {
            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(services);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }

        /// <summary>
        /// 向Memcache中写数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(string key, object value)
        {
            mc.Set(key, value);
        }
        /// <summary>
        ///  向Memcache中写数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time">过期时间</param>
        public static void Set(string key, object value, DateTime time)
        {
            mc.Set(key, value, time);
        }
        /// <summary>
        /// 获取Memcahce中的数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 删除Memcache中的数据
        /// </summary>
        /// <param name="key"></param>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                return mc.Delete(key);
            }
            return false;
        }
    }
}
