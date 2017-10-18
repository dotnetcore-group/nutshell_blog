/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.ViewModel
 * 文件名：UserLogin
 * 版本号：V1.0.0.0
 * 唯一标识：03de2dc7-c6ef-47d7-9ed9-3d1567da0925
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 13:57:04
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 13:57:04
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class UserLogin
    {
        [DisplayName("账号")]
        [Required(ErrorMessage ="账号不能为空！")]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [Required(ErrorMessage = "密码不能为空！")]
        public string Password { get; set; }

        [DisplayName("记住我？")]
        public bool RememberMe { get; set; }
    }
}
