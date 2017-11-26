using Nutshell.Blog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class JobController : Controller
    {
        // GET: Job
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Search(string kw)
        {
            var url = $"http://sou.zhaopin.com/jobs/searchresult.ashx?jl=北京AC&kw={kw}&sm=0&p=1";
            var jobs = JobSearch.GetJobs(url, Core.Model.SourceType.ZLZP);
            return Json(jobs);
        }
    }
}