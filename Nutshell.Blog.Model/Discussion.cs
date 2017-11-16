/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Discussion
 * 版本号：V1.0.0.0
 * 唯一标识：b855d64c-1305-41b9-a814-0aaa7083cd46
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 15:02:23
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 15:02:23
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
    [Table("discussion")]
    public class Discussion
    {
        public Discussion()
        {
            Discussion_Time = DateTime.Now;
        }
        [Key]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Content { get; set; }

        public DateTime? Discussion_Time { get; set; }

        [ForeignKey("User")]
        public int User_Id { get; set; }

        [ForeignKey("Article")]
        public int Article_Id { get; set; }

        [ForeignKey("ParentDiscussion")]
        public int? Reply_Id { get; set; }

        public virtual ICollection<Discussion> Reply { get; set; }
        public virtual Discussion ParentDiscussion { get; set; }
        public virtual User User { get; set; }
        public virtual Article Article { get; set; }
    }
}
