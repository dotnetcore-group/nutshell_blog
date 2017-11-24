using Nutshell.Blog.Common;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Nutshell.Blog.Core.Filters
{
    /// <summary>
    /// 权限过滤
    /// </summary>
    public class SupportFilterAttribute : CheckUserLoginAttribute
    {
        public string Action { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            // 获得请求的 area / controller / action
            var areaName = filterContext.RouteData.DataTokens["area"]?.ToString();
            var controllerName = filterContext.RouteData.Values["controller"]?.ToString();
            var actionName = filterContext.RouteData.Values["action"]?.ToString();

            // 获取URL路径
            string filePath = HttpContext.Current.Request.FilePath;
            Account account = Account;

            ValiddatePermission(filterContext, account, areaName, controllerName, actionName, filePath);
        }

        private void ValiddatePermission(ActionExecutingContext filterContext, Account account, string areaName, string controller, string action, string filePath)
        {
            action = string.IsNullOrEmpty(Action) ? action : Action;
            var result = false;

            if (account != null)
            {
                if (!string.IsNullOrEmpty(areaName))
                {
                    // controller : area/controller
                    controller = $"{areaName}/{controller}";
                }

                // 用户所有的权限
                IList<Permission> permission = null;
                // TODO : 取存在缓存里的数据
                permission = filterContext.HttpContext.Session[filePath + account.User_Id] as IList<Permission>;
                if (permission == null)
                {
                    RightRepository rightRepository = new RightRepository();
                    permission = rightRepository.GetPermission(account.User_Id, controller);
                    // TODO : 放入缓存
                    filterContext.HttpContext.Session[filePath + account.User_Id] = permission;
                }

                //查询当前Action 是否有操作权限，大于0表示有，否则没有
                int count = permission.Where(a => a.KeyCode.Equals(action, StringComparison.CurrentCultureIgnoreCase)).Count();
                result = count > 0;
                if (!result)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.Result = new JsonResult() { Data = new { code=1,msg= "你没有操作权限，请联系管理员！" } };
                        return;
                    }
                    filterContext.HttpContext.Response.StatusCode = 403;
                    filterContext.Result = new ContentResult() { Content = "你没有操作权限，请联系管理员！" };
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/account/signin");
                return;
            }
        }
    }
}