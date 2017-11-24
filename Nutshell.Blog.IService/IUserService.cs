using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IUserService : IBaseService<User>
    {
        User ValidationUser(string userName, string password, out string msg);

        User ChangePassword(int userId, string oldPwd, string newPwd, out string msg);
    }
}
