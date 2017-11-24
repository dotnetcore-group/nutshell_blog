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
using Newtonsoft.Json;
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

        [Key, Column(Order = 1)]
        public int User_Id { get; set; }

        [Key, Column(Order = 2), ForeignKey("Article")]
        public int Article_Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Url { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        public DateTime? Collection_Time { get; set; }

        [MaxLength(200)]
        public string Remark { get; set; }

        [JsonIgnore]
        [ForeignKey("User_Id")]
        public virtual User User { get; set; }
        [JsonIgnore]
        public virtual Article Article { get; set; }
    }
}
