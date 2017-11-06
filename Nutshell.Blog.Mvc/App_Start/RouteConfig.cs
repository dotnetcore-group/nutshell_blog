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
                defaults: new { controller = "Article", action = "Detail" },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" },
                constraints: new { }
            );
            routes.MapRoute(
                name: "BlogHome",
                url: "{author}",
                defaults: new { controller = "Article", action = "Blogs" },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" }
            );

            routes.MapRoute(
                name: "ArticleList",
                url: "all/{index}",
                defaults: new { controller = "Home", action = "All", index = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" },
                constraints: new { }
            );
            
            routes.MapRoute(
                name: "UserHome",
                url: "u/{id}",
                defaults: new { controller = "Account", action = "UserHome" }
            );
            routes.MapRoute(
                name: "Category",
                url: "cate/{category}/{index}",
                defaults: new { controller = "Home", action = "Category", index = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "All", id = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" }
            );
        }
    }
}
