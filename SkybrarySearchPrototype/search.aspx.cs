using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Highlight;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System.Text;
using Directory=Lucene.Net.Store.Directory;


namespace SkybrarySearchPrototype
{
    public partial class search : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            
        }

        public string GeneratePreviewText(Query q, string text)
        {
            QueryScorer scorer = new QueryScorer(q);
            Formatter formatter = new SimpleHTMLFormatter("<span style='background-color:#FAEAAA'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new SimpleFragmenter(100));
            TokenStream stream = new StandardAnalyzer().TokenStream("content", new StringReader(text));
            return highlighter.GetBestFragment(stream, text);
        }
    }
}
