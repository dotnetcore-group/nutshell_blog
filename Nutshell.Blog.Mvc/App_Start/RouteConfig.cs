using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nutshell.Blog.Mvc
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "ArticleDetail",
                url: "{author}/p/{id}.html",
                defaults: new { controller = "Article", action = "Detail", id = UrlParameter.Optional },
                constraints: new { }
            );
            routes.MapRoute(
                name: "UserCenter",
                url: "{author}",
                defaults: new { controller = "Account", action = "Home" }
            );
            routes.MapRoute(
                name: "ArticleList",
                url: "all/{index}",
                defaults: new { controller = "Home", action = "All", index = UrlParameter.Optional },
                constraints: new { }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "All", id = 1 }
            );
        }
    }
}
