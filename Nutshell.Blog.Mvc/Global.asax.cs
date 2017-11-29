using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using log4net;
using System.Web.Routing;
using Nutshell.Blog.Mvc.Controllers;

namespace Nutshell.Blog.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly string[] staticFileExt = new string[] { ".axd", ".ashx", ".bmp", ".css", ".gif", ".ico", ".jpeg", ".jpg", ".js", ".png", ".rar", ".zip", ".woff", ".ttf", ".eot", ".svg" };
        protected void Application_Start()
        {
            //log4net.Config.XmlConfigurator.Configure();//读取Log4Net配置信息
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FilterConfig.Register(GlobalFilters.Filters);

            AutofacConfig.Register();

            //屏蔽浏览器中的ASP.NET版本
            MvcHandler.DisableMvcResponseHeader = true;
            RemoveWebFormEngines();

            //RecordLog();

            PanGuLuceneHelper.Instance.StartThread();
        }

        protected void Application_Error()
        {
            //var error = Server.GetLastError() as HttpException;
            //if (error != null)
            //{
            //    if (!IsStaticResource(Request))
            //    {
            //        Response.Clear();
            //        Server.ClearError();
            //        Response.TrySkipIisCustomErrors = true;
            //        IController controller = new ErrorController();
            //        var routeData = new RouteData();
            //        routeData.Values.Add("controller", "Error");
            //        var code = error.GetHttpCode();
            //        if (code == 404)
            //        {
            //            routeData.Values.Add("action", "NotFound");
            //        }
            //        Response.StatusCode = code;
            //        controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
            //    }
            //}
        }

        private bool IsStaticResource(HttpRequest request)
        {
            string extension = VirtualPathUtility.GetExtension(request.Path);
            return staticFileExt.Contains(extension);
        }

        // redis分布式日志记录
        void RecordLog()
        {
            ThreadPool.QueueUserWorkItem((obj) =>
            {
                while (true)
                {
                    if (GlobalExceptionAttribute.client.GetListCount(Keys.Exception) > 0)
                    {
                        string exception = GlobalExceptionAttribute.client.DequeueItemFromList(Keys.Exception);
                        ILog logger = LogManager.GetLogger(Keys.Exception);
                        //logger.Error(exception);
                    }
                    else
                    {
                        Thread.Sleep(3000);
                    }
                }
            });
        }

        /// <summary>
        /// 移除web form视图引擎
        /// </summary>
        void RemoveWebFormEngines()
        {
            var engines = ViewEngines.Engines;
            var webFormEngines = engines.OfType<WebFormViewEngine>().FirstOrDefault();
            if (webFormEngines != null)
            {
                engines.Remove(webFormEngines);
            }
        }
    }
}
