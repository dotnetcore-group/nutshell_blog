using Nutshell.Blog.Common;
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class CustomCategoryService : BaseService<CustomCategory>, ICustomCategoryService
    {
        ICustomCategoryRepository customCategoryRepository;
        public CustomCategoryService(ICustomCategoryRepository customCategoryRepository)
        {
            this.customCategoryRepository = customCategoryRepository;
            baseDal = customCategoryRepository;
        }

        /// <summary>
        /// 获得用户的所有自定义分类及文章数量
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<CustomCategories> GetCustomCategoriesByUserId(long UserId)
        {
            return customCategoryRepository.ExecuteSelectQuery<CustomCategories>("[dbo].[PROC_GET_CustomCategoriesALL] @User_Id=@User_Id", new System.Data.SqlClient.SqlParameter("@User_Id", UserId));
        }
    }
}
