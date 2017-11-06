using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.ViewModel
{
    /// <summary>
    /// signalR 用户
    /// </summary>
    public class HubUser
    {
        /// <summary>
        /// signalr连接后的id
        /// </summary>
        public string ConnectionId { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户所在组的id
        /// </summary>
        public string GroupId { get; set; }
    }
}
