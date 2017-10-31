using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using Nutshell.Blog.IReposotory;

namespace Nutshell.Blog.Service
{
    public class MessageService : BaseService<Message>, IMessageService
    {
        IMessageRepository currentRepository;
        public MessageService(IMessageRepository repository)
        {
            base.baseDal = repository;
            currentRepository = repository;
        }
    }
}
