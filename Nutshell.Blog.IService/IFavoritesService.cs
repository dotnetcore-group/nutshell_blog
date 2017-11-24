using Nutshell.Blog.IService;
using Nutshell.Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.IService
{
    public interface IFavoritesService : IBaseService<Favorites>
    {
        bool HaveCollection(int user_id, int article_id);

        List<Favorites> CurrentUserFavorites(int userId);
    }
}
