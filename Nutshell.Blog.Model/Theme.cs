/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：Theme
 * 版本号：V1.0.0.0
 * 唯一标识：688fa6db-44af-46c1-86f0-77be881ed4a1
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 19:03:52
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 19:03:52
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
    [Table("sys_theme")]
    public class Theme
    {
        [Key]
        public int Id { get; set; }

        public string ThemeName { get; set; }

        public string Resources { get; set; }

        public string Remark { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
