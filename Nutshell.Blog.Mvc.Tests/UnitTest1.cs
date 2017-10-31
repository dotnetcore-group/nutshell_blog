using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nutshell.Blog.Model;
using System.Linq;

namespace Nutshell.Blog.Mvc.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (NutshellBlogContext db = new NutshellBlogContext())
            {
                var send = db.User.Where(u => u.User_Id == 3).FirstOrDefault();
                var receive = db.User.Where(u => u.User_Id == 1001).FirstOrDefault();

                db.Message.Add(new Message { Sender = send, Recipient = receive, Title = "消息头部", Content = "这是一条消息" });
                var res = db.SaveChanges();
                Assert.AreEqual(1, res);
            }
        }
    }
}
