/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Common
 * 文件名：DateTimeUtil
 * 版本号：V1.0.0.0
 * 唯一标识：54d8b073-b761-4822-9c8f-a1c2772fb143
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-25 20:14:08
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-25 20:14:08
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
    public static class DateTimeUtil
    {
        public static string GetYearMonthDayString(this DateTime expires)
        {
            try
            {
                var res = new StringBuilder();
                var now = DateTime.Now;
                var timespan = now - expires;
                var year = 0;
                var month = 0;
                var day = 0;
                var total = (int)(timespan.TotalDays);
                year = total / 365;
                if (year > 0)
                {
                    res.Append($"{year}年");
                }

                day = total - (year * 360);
                month = day / 30;

                if (month > 0)
                {
                    res.Append($"{month}个月");
                }
                day -= month * 30;
                if (day > 0)
                {
                    res.Append($"{day}天");
                }

                if (res.Length <= 0)
                {
                    res.Append("未知");
                }
                return res.ToString();
            }
            catch
            {

            }
            return "0";
        }
    }
}
