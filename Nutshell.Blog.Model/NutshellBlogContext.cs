/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Model
 * 文件名：NutshellBlogContext
 * 版本号：V1.0.0.0
 * 唯一标识：0900718f-acb0-4751-95b8-5677f32056be
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 12:30:56
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 12:30:56
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model
{
    public class NutshellBlogContext : DbContext
    {
        public NutshellBlogContext() : base("name=NutshellBlogEntities") { }

        public DbSet<User> User { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<Discussion> Discussion { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<CustomCategory> CustomCategory { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Dictionaries> Dictionaries { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<Right> Right { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<RightOperate> RightOperate { get; set; }
        public DbSet<ModuleOperate> ModuleOperate { get; set; }
        public DbSet<ArticleState> ArticleState { get; set; }
        public DbSet<Article_Category> Article_Category { get; set; }
        public DbSet<FriendLinks> FriendLinks { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasMany(u => u.Discussions).WithRequired(d => d.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(u => u.Favorites).WithRequired(f => f.User).WillCascadeOnDelete(false);
            //modelBuilder.Entity<User>().HasMany(u => u.SendMessages)
            //            .WithRequired(m => m.Sender);
            //modelBuilder.Entity<User>().HasMany(u => u.ReceiveMessages)
            //            .WithRequired(m => m.Recipient);
            base.OnModelCreating(modelBuilder);

        }
    }
}
