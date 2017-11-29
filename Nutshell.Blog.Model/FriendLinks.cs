using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_friend_link")]
    public class FriendLinks
    {
        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        [StringLength(10)]
        public string Text { get; set; }

        [StringLength(100)]
        public string Logo { get; set; }

        [Required]
        [StringLength(50)]
        public string Link { get; set; }

        public int? Sort { get; set; }
    }
}
