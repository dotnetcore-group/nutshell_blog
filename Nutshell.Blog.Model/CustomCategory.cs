/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：UserArticleCategory
 * 版本号：V1.0.0.0
 * 唯一标识：6849fefc-032f-4bff-bf2e-daa81dfe7b99
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 18:46:52
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 18:46:52
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
    [Table("custom_category")]
    public class CustomCategory
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(20)]
        public string CategoryName { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual User Author { get; set; }
    }
}
