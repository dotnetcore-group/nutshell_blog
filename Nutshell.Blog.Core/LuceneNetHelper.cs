/*********************************************************************************
 * Copying (c) 2017 All Rights Reserved.
 * CLR版本：4.0.30319.42000
 * 机器名称：ASUS_PC
 * 公司名称：
 * 命名空间：Nutshell.Blog.Core
 * 文件名：LuceneNetHelper
 * 版本号：V1.0.0.0
 * 唯一标识：79c7e852-82c8-436f-9e58-df1fe2776c69
 * 创建人：曾安德
 * 电子邮箱：zengande@qq.com
 * 创建时间：2017-10-16 16:30:01
 * 
 * 描述：
 * 
 * ===============================================================================
 * 修改标记
 * 修改时间：2017-10-16 16:30:01
 * 修改人：曾安德
 * 版本号：V1.0.0.0
 * 描述：
 * 
 *********************************************************************************/
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Tokenattributes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutshell.Blog.Core
{
    public class LuceneNetHelper
    {
        public static readonly string IndexPath =ConfigurationManager.AppSettings["lucenedir"];

        public static string CreateHightLight(string keywords, string content)
        {
            PanGu.HighLight.SimpleHTMLFormatter simpleHTMLFormatter = new PanGu.HighLight.SimpleHTMLFormatter("<font color='red'>","</font>");
            PanGu.HighLight.Highlighter highlighter = new PanGu.HighLight.Highlighter(simpleHTMLFormatter, new PanGu.Segment());
            highlighter.FragmentSize = 150;
            return highlighter.GetBestFragment(keywords, content);
        }

        public static List<string> PanGuSplitWord(string kw)
        {
            Analyzer analyzer = new PanGuAnalyzer();
            TokenStream tokenStream = analyzer.TokenStream("", new StringReader(kw));
            List<string> list = new List<string>();
            while (tokenStream.IncrementToken())
            {
                var termAttribute = tokenStream.GetAttribute<ITermAttribute>();
                list.Add(termAttribute.Term);
            }
            return list;
        }
    }
}
