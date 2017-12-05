/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model.ViewModel
 * 文件名：JsonModel
 * 版本号：V1.0.0.0
 * 唯一标识：009b7a34-220e-4d9b-a11d-02217e414ab1
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-07 21:43:45
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-07 21:43:45
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

namespace Nutshell.Blog.ViewModel
{
    /// <summary>
    ///  返回json值对象
    /// </summary>
    public class JsonModel
    {
        public int code { get; set; }
        public string msg { get; set; }
        public object res { get; set; }
        public int total { get; set; }
        public int index { get; set; }
        public int size { get; set; }
    }
}
