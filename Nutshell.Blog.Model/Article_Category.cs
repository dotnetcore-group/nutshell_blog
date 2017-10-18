/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Article_Category
 * 版本号：V1.0.0.0
 * 唯一标识：6a47719b-68fd-4ff1-9c96-89f9bf19935e
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 15:34:56
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 15:34:56
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_article_category")]
    public class Article_Category
    {
        public Article_Category()
        {
            Sort = 0;
        }

        [Key]
        public int Cate_Id { get; set; }

        public string Cate_Name { get; set; }

        [ForeignKey("Categories")]
        public int? Parent_Id { get; set; }

        public int Sort { get; set; }

        public virtual ICollection<Article_Category> Categories { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
