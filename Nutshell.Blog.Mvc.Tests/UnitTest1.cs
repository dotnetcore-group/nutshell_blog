using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nutshell.Blog.Model;
using System.Linq;
using HtmlAgilityPack;
using System.Text;

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
                //var send = db.User.Where(u => u.User_Id == 3).FirstOrDefault();
                //var receive = db.User.Where(u => u.User_Id == 1001).FirstOrDefault();

                //db.Message.Add(new Message { Sender = send, Recipient = receive, Title = "消息头部", Content = "这是一条消息" });
                //var res = db.SaveChanges();
                //Assert.AreEqual(1, res);
            }
        }

        [TestMethod]
        public void TestMethod2()
        {
            var htmlWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.GetEncoding("UTF-8")
            };
            HtmlAgilityPack.HtmlDocument response = htmlWeb.Load("http://sou.zhaopin.com/jobs/searchresult.ashx?jl=%E5%8C%97%E4%BA%AC&kw=.net&sm=0&p=1");

            var divs = response.DocumentNode.SelectNodes("//*[@id='newlist_list_content_table']/table[@class='newlist']");
            for (int i = 0; i < divs.Count; i++)
            {
                var item = divs[i];
                var xpath = $"/html[1]/body[1]/div[4]/div[3]/div[3]/form[1]/div[1]/div[1]/div[2]/div[1]/table[{i+2}]";
                string titleName, infourl, company, city, date, salary, salary_em, source;
                //titleName = item.SelectSingleNode(xpath + "/tbody/tr[1]/td[1]/div/a").Attributes["title"].Value;
                infourl = item.SelectNodes(xpath + "/tbody/tr/td[@class='zwmc']/div/a")?[0].Attributes["href"].Value;
                company = item.SelectSingleNode(xpath + "/tbody/tr/td[@class='gemc']/a")?.InnerText;
                city = item.SelectSingleNode(xpath + "/tbody/tr[1]/td[@class='gzdd']")?.InnerText;
                date = item.SelectSingleNode(xpath + "/tbody/tr[1]/td[@class='gxsj']/span")?.InnerText;
                salary = item.SelectSingleNode(xpath + "/tbody/tr[1]/td[@class='zwyx']")?.InnerText;
                //salary_em = item.SelectSingleNode(xpath + "/a/dl/dt[@class='salary']/em").InnerText;
                source = "智联招聘";

                //zpInfoList.Add(
                //    new ZhaopinInfo()
                //    {
                //        city = city,
                //        company = company,
                //        date = date,
                //        info_url = infourl,
                //        salary = salary,
                //        salary_em = salary_em,
                //        titleName = titleName,
                //        source = source
                //    });
            }
        }
    }
}
