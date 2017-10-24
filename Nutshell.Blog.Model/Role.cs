/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Role
 * 版本号：V1.0.0.0
 * 唯一标识：6d137f5b-8c12-4f51-9a16-bef2c9969d0c
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-21 19:04:13
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-21 19:04:13
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
    [Table("sys_role")]
    public class Role
    {
        public Role()
        {
            IsDeleted = false;
        }

        [Key]
        public int Role_Id { get; set; }

        [MaxLength(10)]
        [Required]
        public string Role_Name { get; set; }

        [MaxLength(100)]
        public string Img_Icon { get; set; }

        public bool? IsDeleted { get; set; }

        [MaxLength(200)]
        public string Remark { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
