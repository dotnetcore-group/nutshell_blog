/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.ViewModel
 * 文件名：Account
 * 版本号：V1.0.0.0
 * 唯一标识：071af947-afe9-4a6b-a524-588541427ae7
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-18 15:33:49
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-18 15:33:49
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class Account
    {
        public int User_Id { get; set; }

        public string UserName { get; set; }

        public string Nickname { get; set; }

        public int ThemeId { get; set; }

        public IList<CustomCategory> CustomCategories { get; set; }
    }
}
