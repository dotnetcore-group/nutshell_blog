/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Favorites
 * 版本号：V1.0.0.0
 * 唯一标识：7a566883-4b2c-4629-bd58-2b99bccac689
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 15:17:26
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 15:17:26
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
    [Table("favorites")]
    public class Favorites
    {
        public Favorites()
        {
            Collection_Time = DateTime.Now;
        }

        [Key, Column(Order = 1), ForeignKey("User")]
        public int User_Id { get; set; }

        [Key, Column(Order = 2), ForeignKey("Article")]
        public int Article_Id { get; set; }

        public DateTime? Collection_Time { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public virtual User User { get; set; }
        public virtual Article Article { get; set; }
    }
}
