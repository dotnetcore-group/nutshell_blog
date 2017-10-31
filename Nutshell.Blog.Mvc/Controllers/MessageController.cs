using Nutshell.Blog.IService;
using Nutshell.Blog.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class MessageController : BaseController
    {
        IMessageService msgService;
        public MessageController(IModuleService moduleService)
        {
            base.moduleService = moduleService;
        }

        // GET: Message
        [CheckUserLogin]
        public ActionResult Index()
        {
            //var account = GetCurrentAccount();
            return View();
        }
    }
}