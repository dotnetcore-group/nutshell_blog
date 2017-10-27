using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IArticleService : IBaseService<Article>
    {
        Article AddArticle(Article article);

        List<Archives> GetArchivesByUserId<Archives>(int UserId);

        List<CustomCategories> GetCustomCategoriesByUserId<CustomCategories>(int UserId);
    }
}
