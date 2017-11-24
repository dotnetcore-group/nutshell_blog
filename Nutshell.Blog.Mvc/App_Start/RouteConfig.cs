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
                name: "UserCategory",
                url: "{author}/category/{CategoryId}.html",
                defaults: new { controller = "User", action = "Category" }
            );

            routes.MapRoute(
                name: "ArticleList",
                url: "all/{index}",
                defaults: new { controller = "Home", action = "Index", index = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" },
                constraints: new { }
            );
            
            //routes.MapRoute(
            //    name: "UserHome",
            //    url: "u/me",
            //    defaults: new { controller = "User", action = "UserHome" }
            //);
            //routes.MapRoute(
            //    name: "Favorite",
            //    url: "u/favorite",
            //    defaults: new { controller = "User", action = "Favorite" }
            //);
            routes.MapRoute(
                name: "Category",
                url: "cate/{category}/{index}",
                defaults: new { controller = "Home", action = "Category", index = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "Nutshell.Blog.Mvc.Controllers" }
            );
        }
    }
}
