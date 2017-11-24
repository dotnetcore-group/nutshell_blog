using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    public class CustomCategories
    {
        public CustomCategories()
        {

        }
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public int Count { get; set; }
    }
}
