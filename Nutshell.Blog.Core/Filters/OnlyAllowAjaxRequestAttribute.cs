using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nutshell.Blog.Core.Filters
{
    public class OnlyAllowAjaxRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                return;
            }
            filterContext.Result = new HttpNotFoundResult();
            return;
        }
    }
}
