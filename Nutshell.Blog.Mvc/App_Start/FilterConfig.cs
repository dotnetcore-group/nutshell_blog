using Nutshell.Blog.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc
{
    public class FilterConfig
    {
        public static void Register(GlobalFilterCollection filters)
        {
            //filters.Add(new CheckUserLoginAttribute());
            //filters.Add(new SupportFilterAttribute());
            filters.Add(new GlobalExceptionAttribute());
        }
    }
}