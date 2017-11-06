using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Nutshell.Blog.Core;
using Nutshell.Blog.Mvc.Controllers;
using Nutshell.Blog.Common;

namespace Nutshell.Blog.Mvc.Hubs
{
    public class PushHub : Hub
    {
        public override Task OnConnected()
        {
            AddUser();
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            OnlineUserPool.DeleteUser(new Model.ViewModel.HubUser {
                ConnectionId = Context.ConnectionId
            });
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            AddUser();
            return base.OnReconnected();
        }

        private void AddUser()
        {
            OnlineUserPool.AddUser(new Model.ViewModel.HubUser()
            {
                ConnectionId = Context.ConnectionId,
                UserId = GetUserId()
            });
        }

        string GetUserId()
        {
            return Context.RequestCookies[Keys.UserId]?.Value;
        }

        public void Online(string userId)
        {
            Clients.Others.SayHello("Hello");
            OnlineUserPool.AddUser(new Model.ViewModel.HubUser() {
                ConnectionId = Context.ConnectionId,
                UserId = userId
            });
        }

        public void AuditPassed(string userid)
        {
            var user = OnlineUserPool.OnlineUsers.FirstOrDefault(u => u.UserId == userid);
            // 用户在线
            if (user != null)
            {
                var client = Clients.Client(user.ConnectionId);
                
                client.Receive("您的文章通过了审核！");
            }

        }
    }
}