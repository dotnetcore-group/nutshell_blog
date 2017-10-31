using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_right")]
    public class Right
    {
        [Key]
        public int Id { get; set; }

        public int Module_Id { get; set; }

        public int Role_Id { get; set; }

        public bool RightFlag { get; set; }

        [ForeignKey("Module_Id")]
        public virtual Module Module { get; set; }
        [ForeignKey("Role_Id")]
        public virtual Role Role { get; set; }
        public virtual ICollection<RightOperate> RightOperates { get; set; }
    }
}
