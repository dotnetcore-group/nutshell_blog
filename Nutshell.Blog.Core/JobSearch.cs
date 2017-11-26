using HtmlAgilityPack;
using Nutshell.Blog.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public static class JobSearch
    {
        public static List<Job> GetJobs(string url, SourceType sourceType)
        {
            var htmlWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.GetEncoding("UTF-8")
            };
            var jobs = new List<Job>();
            switch (sourceType)
            {
                case SourceType.ZLZP:
                    {
                        HtmlWeb.PreRequestHandler preRequestHandler = new HtmlAgilityPack.HtmlWeb.PreRequestHandler((request) =>
                        {
                            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
                            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                            request.CookieContainer = new System.Net.CookieContainer();
                            return true;
                        });
                        htmlWeb.PreRequest += preRequestHandler;
                        HtmlDocument response = htmlWeb.Load(url);
                        var tables = response.DocumentNode.SelectNodes("//*[@id='newlist_list_content_table']/table[@class='newlist']");
                        if (tables != null)
                        {
                            Job job = null;
                            foreach (var table in tables)
                            {
                                job = new Job();
                                //item.SelectSingleNode("tr[1]/td[@class='zwyx']").InnerText
                                //"6001-8000"
                                //item.SelectSingleNode("tr[1]/td[@class='zwmc']/div/a").InnerText
                                //"C#/Asp.Net软件工程师，研发工程师"
                                //item.SelectSingleNode("tr[1]/td[@class='gzdd']").InnerText
                                //"北京"
                                //item.SelectSingleNode("tr[1]/td[@class='gxsj']").InnerText
                                //"最新"
                                //item.SelectSingleNode("tr[1]/td[@class='gsmc']/a[1]").InnerText
                                //"北京乐鸟科技有限公司"
                                job.Name = table.SelectSingleNode("tr[1]/td[@class='zwmc']/div/a")?.InnerText;
                                if (job.Name == null) { continue; }
                                job.Link = table.SelectSingleNode("tr[1]/td[@class='zwmc']/div/a")?.Attributes["href"]?.Value;
                                job.City = table.SelectSingleNode("tr[1]/td[@class='gzdd']")?.InnerText;
                                job.Date = table.SelectSingleNode("tr[1]/td[@class='gxsj']")?.InnerText;
                                job.Company = table.SelectSingleNode("tr[1]/td[@class='gsmc']/a[1]")?.InnerText;
                                job.Wages = table.SelectSingleNode("tr[1]/td[@class='zwyx']")?.InnerText;
                                job.Source = "智联招聘";

                                jobs.Add(job);
                            }
                        }
                        break;
                    }
            }
            return jobs;
        }
    }
}
