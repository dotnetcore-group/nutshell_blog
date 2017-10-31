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
            Id = Guid.NewGuid();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SendTime { get; set; }

        public bool IsRead { get; set; }

        /// <summary>
        /// 发送者
        /// </summary>
        public virtual User Sender { get; set; }
        /// <summary>
        /// 接收者
        /// </summary>
        [Required]
        public virtual User Recipient { get; set; }
    }
}
