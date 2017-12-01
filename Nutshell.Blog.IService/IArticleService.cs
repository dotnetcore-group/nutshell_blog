using Nutshell.Blog.Model;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IArticleService : IBaseService<Article>
    {
        Article AddArticle(Article article);

        List<Archives> GetArchivesByUserId<Archives>(long UserId);

        List<CustomCategories> GetCustomCategoriesByUserId<CustomCategories>(long UserId);

        int GetArticlesTotalCount(long UserId);

        List<Article> LoadPageEntities(Expression<Func<Article, bool>> where, int pageIndex, int pageSize);
    }
}
