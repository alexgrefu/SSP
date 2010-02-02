using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Highlight;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;

namespace SkybrarySearch.Index
{
    public class SearchQuery
    {      
        public string Query { get; set; }
        public IList<String> Filters { get; set; }
        public IndexSearcher Searcher { get; set; }

        private Query GetQuery()
        {
            var queryParser = new MultiFieldQueryParser
                (new[] { "Title", "Content" }, new StandardAnalyzer());
            return queryParser.Parse(Query);
        }

        public TopDocs Search(int pageSize)
        {
            Query query = GetQuery();

            TopDocs topDocs;
            if (Filters != null)
            {
                BooleanQuery filterQuery = new BooleanQuery();

                foreach (var id in Filters)
                {
                    filterQuery.Add(
                        new TermQuery(
                            new Term("IcaoId", id)),
                            BooleanClause.Occur.SHOULD);
                }

                var filter = new QueryFilter(filterQuery); //new SkybraryFilter(query.Filters.ToArray());
                topDocs = Searcher.Search(query, filter, pageSize);
            }
            else
            {
                topDocs = Searcher.Search(query, null, 5000);
            }

            return topDocs;
        }

        public string HighlightContent(string text)
        {
            QueryScorer scorer = new QueryScorer(GetQuery());
            Formatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new SimpleFragmenter(120));
            TokenStream stream = new StandardAnalyzer().TokenStream("Content", new StringReader(text));

            var fragments = highlighter.GetBestFragments(stream, text, 3);

            if (fragments == null || fragments.Length == 0) return text.Length > 120 ? text.Substring(0, 120) + "..." : text;

            string highlighted = "";

            foreach (var fragment in fragments)
            {

                if (text.StartsWith(fragment))
                    highlighted += "<p>" + fragment + " ... </p>";
                else if (text.EndsWith(fragment))
                    highlighted += "<p> ... " + fragment + "</p>";
                else
                    highlighted += "<p> ... " + fragment + " ... </p>";
            }

            return highlighted;
        }

        public string HighlightTitle(string text)
        {
            QueryScorer scorer = new QueryScorer(GetQuery());

            Formatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new NullFragmenter());
            TokenStream stream = new StandardAnalyzer().TokenStream("Title", new StringReader(text));
            var title = highlighter.GetBestFragment(stream, text);
            return title ?? text;
        }
    }
}
