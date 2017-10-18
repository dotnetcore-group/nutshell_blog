/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Repository
 * 文件名：ArticleRepository
 * 版本号：V1.0.0.0
 * 唯一标识：432f711f-263e-4181-9934-d4a589a716a6
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 16:38:04
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 16:38:04
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Repository
{
    public class ArticleRepository : BaseRepository<Article>, IArticleRepository
    {

    }
}
