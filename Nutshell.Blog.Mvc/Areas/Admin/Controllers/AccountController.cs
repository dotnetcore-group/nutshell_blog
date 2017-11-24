using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.Core.Filters;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model.ViewModel;
using Nutshell.Blog.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUserService userService)
        {
            base.userService = userService;
        }

        // GET: Account
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.Return = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult SignIn(UserLogin user)
        {
            string returnurl = Request["returnUrl"] ?? "/";
            string msg = "";
            dynamic data = null;
            if (ModelState.IsValid)
            {
                var userinfo = userService.ValidationUser(user.UserName, user.Password, out msg);
                data = new { code = 1, msg = msg, url = "" };
                // 验证通过
                if (userinfo != null)
                {
                    // 将用户信息存入memcache
                    // 返回客户端一个session id
                    ValidatedUser(userinfo.ToAccount());
                    data = new { code = 0, msg = msg, url = returnurl, userId = userinfo.User_Id };
                }
            }
            return Json(data);
        }

        void ValidatedUser(Account account)
        {
            try
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(account), DateTime.Now.AddHours(1));
                Response.Cookies[Keys.SessionId].Value = sessionid;
                Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                Response.Cookies[Keys.SessionId].HttpOnly = true;
                //Response.Cookies[Keys.UserId].Value = account.User_Id.ToString();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public ActionResult Register(string returnUrl)
        {
            ViewBag.Return = returnUrl;
            return View();
        }

        [HttpPost]
        [CheckUserLogin]
        public JsonResult ChangePwd(string oldPwd, string newPwd)
        {
            string msg = string.Empty;
            var user = userService.ChangePassword(Account.User_Id, oldPwd, newPwd, out msg);

            var data = new { code = 1, msg = msg };
            if (user != null)
            {
                var sessionid = Guid.NewGuid().ToString();
                MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(user.ToAccount()), DateTime.Now.AddHours(1));
                Response.Cookies[Keys.SessionId].Value = sessionid;
                Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                data = new { code = 0, msg = "修改成功！" };
            }

            return Json(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Register(UserRegister user)
        {
            string returnurl = Request["returnUrl"] ?? "/";
            var res = new JsonModel { code = 1, msg = "注册失败！" };
            if (ModelState.IsValid)
            {
                var userInfo = userService.AddEntity(new Model.User
                {
                    Login_Name = user.UserName,
                    Login_Password = user.Password.Md5_Base64(),
                    Nickname = user.Nickname
                });
                // 注册成功
                if (userService.SaveChanges())
                {
                    res.code = 0;
                    res.res = returnurl;
                    res.msg = "注册成功！";
                    var sessionid = Guid.NewGuid().ToString();
                    MemcacheHelper.Set(sessionid, SerializerHelper.SerializeToString(userInfo.ToAccount()), DateTime.Now.AddHours(1));
                    Response.Cookies[Keys.SessionId].Value = sessionid;
                    Response.Cookies[Keys.SessionId].Expires = DateTime.Now.AddHours(1);
                }
            }
            return Json(res);
        }
        [HttpPost]
        public JsonResult NotExitesUserName()
        {
            string UserName = Request.Params["UserName"];
            if (string.IsNullOrEmpty(UserName))
            {
                return Json(false);
            }
            var user = userService.LoadEntity(u => u.Login_Name.Equals(UserName, StringComparison.CurrentCultureIgnoreCase));
            return user == null ? Json(true) : Json(false);
        }
        public JsonResult NotExitesNickname()
        {
            string Nickname = Request.Params["Nickname"];
            if (string.IsNullOrEmpty(Nickname))
            {
                return Json(false);
            }
            var user = userService.LoadEntity(u => u.Nickname.Equals(Nickname, StringComparison.CurrentCultureIgnoreCase));
            return user == null ? Json(true) : Json(false);
        }
    }
}