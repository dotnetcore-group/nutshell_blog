using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.ViewModel.Article
{
    public class BlogsModel
    {
        public IList<Model.Article> TopArticles { get; set; }

        public IEnumerable<IGrouping<DateTime, Model.Article>> GroupingArticles { get; set; }

        public IList<CustomCategories> CustomCategories { get; set; }

        public IList<Archives> Archives { get; set; }
    }
}
