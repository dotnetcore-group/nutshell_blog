/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Common
 * 文件名：SerializerHelper
 * 版本号：V1.0.0.0
 * 唯一标识：e7ea1f34-6875-49f0-98fb-edaf563682a6
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 15:25:50
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 15:25:50
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Common
{
    public class SerializerHelper
    {
        /// <summary>
        /// 进行序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeToString(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeToObject<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
