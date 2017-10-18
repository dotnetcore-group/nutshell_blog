/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core.Model
 * 文件名：IndexContent
 * 版本号：V1.0.0.0
 * 唯一标识：dfabe044-8f68-43b9-bce3-d3ca347e088b
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 16:04:23
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 16:04:23
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

namespace Nutshell.Blog.Core.Model
{
    public class IndexContent
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Author { get; set; }

        public LuceneType LuceneType { get; set; }

        public DateTime? Creation_Time { get; set; }
    }
}
