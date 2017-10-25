/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.ViewModel
 * 文件名：UserRegister
 * 版本号：V1.0.0.0
 * 唯一标识：01bf4222-ab77-4350-8ec6-5f8ed607834b
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-17 23:45:09
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-17 23:45:09
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nutshell.Blog.Model.ViewModel
{
    public class UserRegister
    {
        [Required(ErrorMessage = "请输入登录用户名！")]
        [DisplayName("登录账号")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "账号至少5位，至多10位！")]
        [Remote("NotExitesUserName", "Account", ErrorMessage = "该账号已被占用！", HttpMethod = "Post")]
        [RegularExpression(@"^\w+$", ErrorMessage ="只能输入数字、字母、下划线！")]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [PasswordPropertyText]
        [Required(ErrorMessage = "请输入密码！")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密码至少8位，至多20位！")]
        public string Password { get; set; }

        [DisplayName("确认密码")]
        [PasswordPropertyText]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "两次输入密码不一致！")]
        public string Repassword { get; set; }

        [DisplayName("昵称")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "昵称至少2位，至多10位！")]
        [Required(ErrorMessage = "请输入昵称！")]
        [Remote("NotExitesNickname", "Account", ErrorMessage = "该昵称已被占用！", HttpMethod = "Post")]
        public string Nickname { get; set; }
    }
}
