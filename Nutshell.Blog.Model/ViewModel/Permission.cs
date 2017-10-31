using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class Permission
    {
        /// <summary>
        /// 操作码
        /// </summary>
        public string KeyCode { get; set; }

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsValid { get; set; }
    }
}
