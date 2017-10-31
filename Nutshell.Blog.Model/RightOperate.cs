using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_rightOperate")]
    public class RightOperate
    {
        public RightOperate()
        {
            IsValid = false;
        }

        [Key]
        public int Id { get; set; }

        public string KeyCode { get; set; }

        public bool IsValid { get; set; }

        public int Right_Id { get; set; }

        [ForeignKey("Right_Id")]
        public virtual Right Right { get; set; }
    }
}
