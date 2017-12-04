using Nutshell.Blog.IService;
using Nutshell.Blog.Core.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nutshell.Blog.Mvc.MvcHelper;

namespace Nutshell.Blog.Mvc.Controllers
{
    public class MessageController : BaseController
    {
        public MessageController(IMessageService msgService)
        {
            messageService = msgService;
        }

        // GET: Message
        // 消息列表
        [CheckUserLogin]
        public ActionResult Index()
        {
            var messages = messageService.LoadEntities(m => m.Recipient.User_Id == Account.User_Id).OrderByDescending(m => m.SendTime).ToList();
            return View(messages);
        }

        public ActionResult Detail(int id)
        {
            return View();
        }

        [CheckUserLogin]
        [HttpPost]
        public JsonResult GetUnreadMessageCount()
        {
            var unreadMessages = messageService.LoadEntities(m => m.Recipient.User_Id == Account.User_Id && !m.IsRead);
            return Json(unreadMessages.Count());
        }
    }
}