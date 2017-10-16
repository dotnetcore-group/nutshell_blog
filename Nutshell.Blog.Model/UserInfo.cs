/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：UserInfo
 * 版本号：V1.0.0.0
 * 唯一标识：f368163f-0349-4200-9218-95ef22e145a9
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 12:42:40
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 12:42:40
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
    [Table("sys_userinfo")]
    public class UserInfo
    {
        public UserInfo()
        {
            Registration_Time = DateTime.Now;
            Nickname = string.Format("用户{0}", new Random().Next(10000, 100000));
            Sex = "保密";
        }

        [Key, ForeignKey("User")]
        public int User_Id { get; set; }

        [StringLength(10)]
        public string Nickname { get; set; }

        [StringLength(2)]
        public string Sex { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime Registration_Time { get; set; }

        [StringLength(50)]
        public string Photo { get; set; }

        public virtual User User { get; set; }
    }
}
