/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Service
 * 文件名：FavoritesService
 * 版本号：V1.0.0.0
 * 唯一标识：aa4f3b96-07ec-4b5d-a83c-da0f6b7f16a7
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-07 21:39:58
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-07 21:39:58
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class FavoritesService : BaseService<Favorites>, IFavoritesService
    {
        IFavoritesRepository favoritesRepository;
        public FavoritesService(IFavoritesRepository favoritesRepository)
        {
            this.favoritesRepository = favoritesRepository;
            baseDal = favoritesRepository;
        }
    }
}
