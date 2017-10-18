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
            LuceneIndexManager.GetInstance().StartThread();
        }
    }
}
