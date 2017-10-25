using Nutshell.Blog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nutshell.Blog.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            FilterConfig.Register(GlobalFilters.Filters);

            AutofacConfig.Register();

            //屏蔽浏览器中的ASP.NET版本
            MvcHandler.DisableMvcResponseHeader = true;
            RemoveWebFormEngines();

            PanGuLuceneHelper.Instance.StartThread();
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
