using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class ResetPwd
    {
        [DisplayName("新密码")]
        [PasswordPropertyText]
        [Required(ErrorMessage = "请输入密码！")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密码至少8位，至多20位！")]
        public string Password { get; set; }

        [DisplayName("确认密码")]
        [PasswordPropertyText]
        [Compare("Password", ErrorMessage = "两次输入密码不一致！")]
        public string Repassword { get; set; }

        [DisplayName("验证码")]
        [Required(ErrorMessage = "请输入验证码！")]
        public string Code { get; set; }
    }
}
