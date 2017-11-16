/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Service
 * 文件名：CategoryService
 * 版本号：V1.0.0.0
 * 唯一标识：833f775a-e9c9-479b-986f-a48dc762753c
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-11-04 21:18:05
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-11-04 21:18:05
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
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
    public class CategoryService : BaseService<Article_Category>, ICategoryService
    {
        ICategoryRepository categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
            baseDal = categoryRepository;
        }

        public List<Article_Category> GetCategories()
        {
            List<Article_Category> categories = null;
            var obj = MemcacheHelper.Get(Keys.Categories);
            if (obj != null)
            {
                categories = SerializerHelper.DeserializeToObject<List<Article_Category>>(obj.ToString());
            }
            if (categories == null)
            {
                categories = categoryRepository.LoadEntities(c => true).OrderBy(c => c.Sort).ToList();
                MemcacheHelper.Set(Keys.Categories, SerializerHelper.SerializeToString(categories));
            }

            return categories;
        }
    }
}
