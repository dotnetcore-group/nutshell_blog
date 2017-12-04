using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_settings")]
    public class Settings
    {
        [Key]
        public long Id { get; set; }

        [DisplayName("名称")]
        [Required]
        public string DisplayName { get; set; }

        [DisplayName("键")]
        [Required, MaxLength(100)]
        [Index(IsUnique = true)]
        public string Key { get; set; }

        [DisplayName("值")]
        [Required]
        public string Value { get; set; }

        public string Remark { get; set; }
    }
}
