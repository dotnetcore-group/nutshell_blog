using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("article_state")]
    public class ArticleState
    {
        [Key]
        public int Id { get; set; }

        public string State { get; set; }
        
        public string Checker { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
