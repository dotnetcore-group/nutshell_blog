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
    public class CustomCategoryService : BaseService<CustomCategory>, ICustomCategoryService
    {
        ICustomCategoryRepository customCategoryRepository;
        public CustomCategoryService(ICustomCategoryRepository customCategoryRepository)
        {
            this.customCategoryRepository = customCategoryRepository;
            baseDal = customCategoryRepository;
        }
    }
}
