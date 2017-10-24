/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Dictionaries
 * 版本号：V1.0.0.0
 * 唯一标识：942b582d-5814-4cdb-a138-5ed60cb5cf02
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-21 19:23:00
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-21 19:23:00
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
    [Table("sys_dictionaries")]
    public class Dictionaries
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Index(IsUnique = true)]
        public string Key { get; set; }

        [Required]
        [MaxLength(20)]
        public string Value { get; set; }

        [MaxLength(200)]
        public string Remark { get; set; }

        public int? Sort { get; set; }

        public virtual ICollection<Dictionaries> Children { get; set; }
        public virtual Dictionaries Parent { get; set; }
    }
}
