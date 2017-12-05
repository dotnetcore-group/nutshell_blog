/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.ViewModel
 * 文件名：SearchArticleResult
 * 版本号：V1.0.0.0
 * 唯一标识：ec73146e-d8e0-4e03-b1cb-fefbefc743ac
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-17 13:46:47
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-17 13:46:47
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

namespace Nutshell.Blog.ViewModel.Article
{
    public class SearchArticleResult
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Creation_Time { get; set; }
        public string Author_Nick { get; set; }
        public string Author_Name { get; set; }
    }
}
