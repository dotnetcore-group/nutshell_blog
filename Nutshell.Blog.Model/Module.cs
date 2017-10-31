using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_module")]
    public class Module
    {
        public Module()
        {
            CreateTime = DateTime.Now;
            IsMenu = false;
            Enable = true;
            Parent_Id = 0;
            IsLast = false;
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public int Parent_Id { get; set; }

        public string Url { get; set; }

        public string Controller { get; set; }

        public string Iconic { get; set; }

        public int Sort { get; set; }

        public string Remark { get; set; }

        public bool Enable { get; set; }

        public string CreateUser { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsMenu { get; set; }

        public int Version { get; set; }

        public bool IsLast { get; set; }

        public virtual ICollection<ModuleOperate> ModuleOperates { get; set; }
        public virtual ICollection<Right> Rights { get; set; }
    }
}
