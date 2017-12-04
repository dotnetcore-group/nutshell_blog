using Newtonsoft.Json;
using Nutshell.Blog.Model.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    [Table("sys_message")]
    public class Message
    {
        public Message()
        {
            IsRead = false;
            SendTime = DateTime.Now;
        }

        [Key]
        public long Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [JsonConverter(typeof(LongDateTimeConvert))]
        public DateTime SendTime { get; set; }

        public bool IsRead { get; set; }
        
        /// <summary>
        /// 接收者
        /// </summary>
        [Required]
        public virtual User Recipient { get; set; }
    }
}
