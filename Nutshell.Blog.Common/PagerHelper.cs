/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Common
 * 文件名：PagerHelper
 * 版本号：V1.0.0.0
 * 唯一标识：1d44ed99-32ec-4fd6-8082-cd4e66cff192
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-04 19:27:25
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-04 19:27:25
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Common
{
    public class PagerHelper
    {
        /// <summary>
        /// 获取总页数
        /// </summary>
        /// <param name="pageSize">每页数据量</param>
        /// <param name="totalCount">总数据量</param>
        /// <returns></returns>
        public static int GetTotalPage(int pageSize, int totalCount)
        {
            return totalCount % pageSize == 0 ? totalCount / pageSize : (totalCount + pageSize) / pageSize;
        }
    }
}
