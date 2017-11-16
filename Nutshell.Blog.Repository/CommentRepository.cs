using Nutshell.Blog.IReposotory;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Repository
{
    public class CommentRepository : BaseRepository<Discussion>, ICommentRepository
    {
    }
}
