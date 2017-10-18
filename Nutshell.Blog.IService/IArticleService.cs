using Nutshell.Blog.Model;
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
    }
}
