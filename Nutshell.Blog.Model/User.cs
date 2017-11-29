/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：User
 * 版本号：V1.0.0.0
 * 唯一标识：9c62119e-144d-4c1d-b53e-fa9575cbb038
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 12:29:32
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 12:29:32
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Newtonsoft.Json;
using Nutshell.Blog.Model.ViewModel;
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
    [Table("sys_user")]
    public class User
    {
        public User()
        {
            IsLock = false;
            IsValid = false;
            Photo = "/upload/photos/default.png";
            Registration_Time = DateTime.Now;
            Theme_Id = 1;

            SendMessages = new HashSet<Message>();
            ReceiveMessages = new HashSet<Message>();
        }

        [Key]
        public int User_Id { get; set; }

        [Required]
        [MaxLength(10)]
        [Index(IsUnique = true)]
        public string Login_Name { get; set; }

        [Required]
        [MaxLength(64)]
        public string Login_Password { get; set; }

        [MaxLength(2)]
        public string Sex { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime Registration_Time { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MaxLength(30)]
        public string Email { get; set; }

        [MaxLength(50)]
        [DisplayName("个性签名")]
        public string Words { get; set; }

        [MaxLength(10)]
        [Index(IsUnique = true)]
        [Required]
        public string Nickname { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        public bool IsLock { get; set; }

        public bool IsValid { get; set; }

        [ForeignKey("Theme")]
        public int Theme_Id { get; set; }

        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Message> SendMessages { get; set; }
        [NotMapped]
        [JsonIgnore]
        public virtual ICollection<Message> ReceiveMessages { get; set; }

        [JsonIgnore]
        public virtual ICollection<Article> Articles { get; set; }
        [JsonIgnore]
        public virtual ICollection<Discussion> Discussions { get; set; }
        [JsonIgnore]
        public virtual ICollection<Favorites> Favorites { get; set; }
        [JsonIgnore]
        public virtual ICollection<CustomCategory> CustomCategories { get; set; }
        [JsonIgnore]
        public virtual Theme Theme { get; set; }
        [JsonIgnore]
        public virtual ICollection<Role> Roles { get; set; }


        public Account ToAccount()
        {
            return new Account
            {
                User_Id = this.User_Id,
                UserName = this.Login_Name,
                Nickname = this.Nickname,
                Photo = Photo,
                IsValid = this.IsValid,
                IsLock = this.IsLock
            };
        }
    }
}
