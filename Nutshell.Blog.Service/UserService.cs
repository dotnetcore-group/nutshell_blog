/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Service
 * 文件名：UserService
 * 版本号：V1.0.0.0
 * 唯一标识：d103ae3b-446f-4680-9fbd-1fcf95fbd043
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 13:30:20
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 13:30:20
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Nutshell.Blog.Common;
using Nutshell.Blog.IReposotory;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Service
{
    public class UserService : BaseService<User>, IUserService
    {
        IUserRepository currentRepository;
        public UserService(IUserRepository userRepository)
        {
            base.baseDal = userRepository;
            currentRepository = userRepository;
        }

        public User ValidationUser(string userName, string password, out string msg)
        {
            var user = currentRepository.LoadEntity(u=>u.Login_Name.Equals(userName, StringComparison.CurrentCultureIgnoreCase));
            if (user != null)
            {
                if (!user.IsLock)
                {
                    if (user.Login_Password.Equals(password.Md5_32()))
                    {
                        msg = "登录成功！";
                    }
                    else
                    {
                        user = null;
                        msg = "用户名或密码错误！";
                    }
                }
                else
                {
                    user = null;
                    msg = "该用户状态异常！";
                }
            }
            else
            {
                msg = "该用户不存在！";
            }
            return user;
        }

        public User ChangePassword(int userId, string oldPwd, string newPwd, out string msg)
        {
            var user = currentRepository.LoadEntity(u=>u.User_Id==userId);
            if (user != null)
            {
                if (user.Login_Password.Equals(oldPwd.Md5_32(), StringComparison.CurrentCultureIgnoreCase))
                {
                    user.Login_Password = newPwd.Md5_32();
                    if (currentRepository.SaveChanges())
                    {
                        msg = "修改成功！";
                        return user;
                    }
                    else
                    {
                        msg = "保存失败！";
                        return null;
                    }
                }
                else
                {
                    msg = "原密码不匹配，修改失败！";
                    return null;
                }
            }
            msg = "修改失败！";
            return null;
        }
    }
}
