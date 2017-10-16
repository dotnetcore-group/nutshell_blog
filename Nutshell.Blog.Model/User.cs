/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：User
 * 版本号：V1.0.0.0
 * 唯一标识：9c62119e-144d-4c1d-b53e-fa9575cbb038
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 12:29:32
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 12:29:32
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    public class User
    {
        public User()
        {
            state = true;
        }

        [Key]
        public int User_Id { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Login_Name { get; set; }

        [Required]
        [StringLength(64, MinimumLength = 6)]
        public string Login_Password { get; set; }

        public bool? state { get; set; }

        public UserInfo UserInfo { get; set; }
    }
}
