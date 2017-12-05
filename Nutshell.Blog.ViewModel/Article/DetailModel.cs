using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.ViewModel.Article
{
    public class DetailModel
    {
        public DetailModel()
        {
            CustomCategories = new List<CustomCategories>();
            Archives = new List<Archives>();
        }

        public Model.Article Article { get; set; }

        public IList<CustomCategories> CustomCategories { get; set; }

        public IList<Archives> Archives { get; set; }
    }
}
