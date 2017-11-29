using Nutshell.Blog.Common;
using Nutshell.Blog.Core;
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class FriendLinksService : BaseService<FriendLinks>, IFriendLinksService
    {
        IFriendLinksRepository friendLinksRepository;
        public FriendLinksService(IFriendLinksRepository friendLinksRepository)
        {
            this.friendLinksRepository = friendLinksRepository;
            baseDal = friendLinksRepository;
        }

        public List<FriendLinks> GetFriendLinks()
        {
            List<FriendLinks> friendLinks = null;

            var obj = MemcacheHelper.Get(Keys.FriendLinks);
            if (obj != null)
            {
                friendLinks = SerializerHelper.DeserializeToObject<List<FriendLinks>>(obj.ToString());
            }
            if (friendLinks == null)
            {
                friendLinks = friendLinksRepository.LoadEntities(f => true).OrderBy(f => f.Sort)?.ToList();
                MemcacheHelper.Set(Keys.FriendLinks, SerializerHelper.SerializeToString(friendLinks), DateTime.Now.AddHours(24));
            }
            return friendLinks;
        }
    }
}
