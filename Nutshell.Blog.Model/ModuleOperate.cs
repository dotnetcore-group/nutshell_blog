using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_moduleOperate")]
    public class ModuleOperate
    {
        public ModuleOperate()
        {
            IsValid = false;
            Sort = 0;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string KeyCode { get; set; }

        public bool IsValid { get; set; }

        public int Sort { get; set; }

        public virtual Module Module { get; set; }
    }
}
