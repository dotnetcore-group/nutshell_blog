using Nutshell.Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    /// <summary>
    /// 在线用户池
    /// </summary>
    public sealed class OnlineUserPool
    {
        private static Lazy<List<HubUser>> _onlineUsers = new Lazy<List<HubUser>>();
        public static List<HubUser> OnlineUsers { get { return _onlineUsers.Value; } }

        public static void AddUser(HubUser user)
        {
            DeleteUser(user);
            _onlineUsers.Value.Add(user);
        }

        public static void DeleteUser(HubUser user)
        {
            var onlineUser = IsOnline(user);
            if (onlineUser != null)
            {
                _onlineUsers.Value.Remove(onlineUser);
            }
        }

        public static HubUser IsOnline(HubUser user)
        {
            var userid = user.UserId;
            var connectionid = user.ConnectionId;
            return _onlineUsers.Value.FirstOrDefault(u => u.UserId == userid || u.ConnectionId == connectionid);
        }
    }
}
