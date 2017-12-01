using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface ICustomCategoryService : IBaseService<CustomCategory>
    {
        List<CustomCategories> GetCustomCategoriesByUserId(long UserId);
    }
}
