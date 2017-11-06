using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Common
{
    public enum ArticleStateEnum
    {
        /// <summary>
        /// 草稿
        /// </summary>
        Draft = 1,
        /// <summary>
        /// 未审核
        /// </summary>
        NotAudited = 2,
        /// <summary>
        /// 已发布
        /// </summary>
        Published = 3,
        /// <summary>
        /// 未通过
        /// </summary>
        NotPass = 4,
        /// <summary>
        /// 已删除
        /// </summary>
        Deleted = 5
    }
}
