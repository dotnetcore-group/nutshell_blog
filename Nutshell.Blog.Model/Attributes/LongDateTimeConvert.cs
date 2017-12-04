using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Model.Attributes
{
    public class LongDateTimeConvert : IsoDateTimeConverter
    {
        public LongDateTimeConvert() : base()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}
