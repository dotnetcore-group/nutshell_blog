/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Repository
 * 文件名：FavoritesRepository
 * 版本号：V1.0.0.0
 * 唯一标识：d410bc22-5d32-4b0a-b0ce-43b5017b7de9
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-07 21:38:37
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-07 21:38:37
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
    public class FavoritesRepository : BaseRepository<Favorites>, IFavoritesRepository
    {

    }
}
