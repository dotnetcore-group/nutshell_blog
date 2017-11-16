/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core.Filters
 * 文件名：GlobalExceptionAttribute
 * 版本号：V1.0.0.0
 * 唯一标识：e2a80b60-5d1a-4947-b76b-b1052b5b69c5
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-08 12:22:10
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-08 12:22:10
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.Common;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nutshell.Blog.Core.Filters
{
    /// <summary>
    /// 全局异常捕获
    /// </summary>
    public class GlobalExceptionAttribute : HandleErrorAttribute
    {
        public static RedisClient client = new RedisClient("127.0.0.1", 6379);

        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            client.EnqueueItemOnList(Keys.Exception, filterContext.Exception.ToString());
        }
    }
}
