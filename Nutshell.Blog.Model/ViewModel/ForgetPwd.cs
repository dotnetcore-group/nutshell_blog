using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class ForgetPwd
    {
        [DisplayName("登录账号")]
        [Required(ErrorMessage = "账号不能为空！")]
        public string UserName { get; set; }

        [DisplayName("验证码")]
        [Required(ErrorMessage = "请输入验证码！")]
        public string Code { get; set; }
    }
}
