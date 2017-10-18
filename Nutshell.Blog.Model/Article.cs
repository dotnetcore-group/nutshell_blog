/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Article
 * 版本号：V1.0.0.0
 * 唯一标识：9aed547f-5447-47fd-b167-77d43aa5ebc0
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 14:50:40
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 14:50:40
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
    [Table("article")]
    public class Article
    {
        public Article()
        {
            Creation_Time = DateTime.Now;
            //Article_State = 1;
        }

        [Key]
        public int Article_Id { get; set; }

        [MaxLength(20)]
        public string Title { get; set; }

        [MaxLength(int.MaxValue)]
        public string Content { get; set; }

        [MaxLength(100)]
        public string Introduction { get; set; }

        public DateTime? Creation_Time { get; set; }

        public DateTime? Edit_Time { get; set; }

        //public int? Article_State { get; set; }

        [ForeignKey("Author")]
        public int? Author_Id { get; set; }

        public virtual User Author { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<Favorites> Favorites { get; set; }
        public virtual ICollection<Article_Category> Categories { get; set; }
        public virtual CustomCategory CustomCategory { get; set; }
    }
}
