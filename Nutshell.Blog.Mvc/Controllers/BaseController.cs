using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class BaseController : Controller
    {
        protected IUserService userService;
        protected IArticleService articleService;
        protected IModuleService moduleService;
        protected IModuleOperateService moduleOperateService;
        protected ICustomCategoryService customCategoryService;
        protected IRoleService roleService;
        protected ICategoryService categoryService;
        protected IFavoritesService favoritesService;
        protected ICommentService commentService;
        protected IFriendLinksService friendLinksService;
        protected IMessageService messageService;

        protected string[] allowedProperties =
        {
                "src", "alt",                           /* <img> */
                "href", "target", "title", "name",       /* <a>   */
                "class"
        };

        protected string[] allowedTags = {
                "p","br",                   /* 段落 换行 */
                "ul","ol","li",             /* 列表 */
                "strong","em","u","s",      /* 粗体 斜体 下划线 中划线 */
                "img","a",                  /* 图像 链接 */
                "pre",                      /* 代码 */
                "quote"                /* 引用 */
        };

        protected Account Account
        {
            get
            {
                return GetCurrentAccount();
            }
        }

        /// <summary>
        /// 获取当前登陆人的账户信息
        /// </summary>
        /// <returns>账户信息</returns>
        protected Account GetCurrentAccount()
        {
            if (Request.Cookies[Keys.SessionId] != null)
            {
                string sessionId = Request.Cookies[Keys.SessionId].Value;//接收从Cookie中传递过来的Memcache的key
                if (sessionId != null)
                {
                    object obj = MemcacheHelper.Get(sessionId);//根据key从Memcache中获取用户的信息
                    return obj == null ? null : SerializerHelper.DeserializeToObject<Account>(obj.ToString());
                }
            }
            Redirect("/account/signin");
            return null;
        }

        protected string filterHtmlInput(string htmlInput)
        {
            return htmlInput
                .FixTags(allowedTags, allowedProperties)
                .FixLinks(whitelistHosts);
                //.ImagesResponsibel();
        }

        protected List<Permission> GetPermission()
        {
            string filePath = HttpContext.Request.FilePath;
            var account = GetCurrentAccount();
            List<Permission> perm = (List<Permission>)Session[filePath + account.User_Id];
            return perm;
        }

        /// <summary>
        /// 白名单域名
        /// </summary>
        protected string[] whitelistHosts = { "localhost", "127.0.0.1", "120.78.88.90" };

        //private static string ImageResponsible(Match imgMatch)
        //{
        //    // retrieve the image from the received Match
        //    string singleImage = imgMatch.Value;

        //    // if the image already has class, return it back as it is
        //    if (Regex.IsMatch(singleImage,
        //                      @"zyf-img class\s*?=\s*?['""]?.*?img-responsive.*?['""]?",
        //                      RegexOptions.IgnoreCase))
        //    {
        //        return singleImage;
        //    }

        //    // if we reached this point, we need to add class="img-responsive" to our image
        //    string newImage = Regex.Replace(singleImage, "<img", @"<img zyf-img class=""img-responsive""",
        //                            RegexOptions.IgnoreCase);
        //    return newImage;
        //}
    }
}