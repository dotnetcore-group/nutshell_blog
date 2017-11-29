using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public static class TemplateHelper
    {
        public static string BuildByFile(string templatePath, NameValueCollection values)
        {
            return BuildByFile(templatePath, values, "[$", "]");
        }

        public static string Build(string template, NameValueCollection values, string prefix, string postfix)
        {
            if (values != null)
            {
                foreach (DictionaryEntry entry in values)
                {
                    template = template.Replace(string.Format("{0}{1}{2}", prefix, entry.Key, postfix), entry.Value.ToString());
                }
            }
            return template;
        }

        public static string BuildByFile(string templatePath, NameValueCollection values, string prefix, string postfix)
        {
            StreamReader reader = null;
            string template = string.Empty;
            try
            {
                reader = new StreamReader(templatePath);
                template = reader.ReadToEnd();
                reader.Close();
                if (values != null)
                {
                    foreach (string key in values.AllKeys)
                    {
                        template = template.Replace(string.Format("{0}{1}{2}", prefix, key, postfix), values[key]);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return template;
        }
    }
}
