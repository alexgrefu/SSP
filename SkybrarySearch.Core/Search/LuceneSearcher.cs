using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SkybrarySearch.Data;

namespace SkybrarySearch.Index
{
    public class LuceneSearcher : ISearcher
    {
        private string pager;
        private readonly SearchQuery searchQuery;

        public string Pager
        {
            get { return pager; }
        }

        public LuceneSearcher(SearchQuery searchQuery) 
            : this(searchQuery, FSDirectory.GetDirectory(HttpContext.Current.Server.MapPath("~/LuceneIndex"))){}

        public LuceneSearcher(SearchQuery searchQuery,Directory directory)
        {
            this.searchQuery = searchQuery;
            searchQuery.Searcher = new IndexSearcher(directory);
        }
      
        public IList<ViewDocument> DoSearch(int pageSize, int currentPage)
        {
            TopDocs docs = searchQuery.Search(pageSize);

            int results = docs.totalHits;

            List<ViewDocument> viewDocs = new List<ViewDocument>();

            SetPager(results, currentPage, pageSize);

            for (int i = (currentPage - 1) * pageSize; i < ((currentPage * pageSize) > results ? results : (currentPage * pageSize)); i++)
            {
                //var expl = searcher.Explain(qry, topDocs.scoreDocs[i].doc);

                var doc = searchQuery.Searcher.Doc(docs.scoreDocs[i].doc);
                var score = docs.scoreDocs[i].score / docs.GetMaxScore();



                viewDocs.Add(new ViewDocument
                {
                    Id = Int32.Parse(doc.Get("Id")),
                    Type = doc.Get("Type"),
                    Content = searchQuery.HighlightContent(doc.Get("Content")),
                    Document = doc.Get("DocumentTitle"),
                    Chapter = doc.Get("Chapter"),
                    Title = searchQuery.HighlightTitle(doc.Get("Title")),
                    OrderInDocument = Int32.Parse(doc.Get("OrderInDocument") ?? "0"),
                    OrderInChapter = Int32.Parse(doc.Get("OrderInChapter") ?? "0"),
                    Score = score
                });

            }
            searchQuery.Searcher.Close();
            return viewDocs;
        }



        public bool DocIsIndexed(int documentId)
        {
            int docs = searchQuery.Searcher.DocFreq(new Term("IcaoId", documentId.ToString()));
            searchQuery.Searcher.Close();
            return docs > 0;
        }


        private void SetPager(int totalResults, int currentPage, int pageSize)
        {
            int totalPages = (int)Math.Ceiling(totalResults / (double)pageSize);
            StringBuilder sb = new StringBuilder();
            sb.Append("Page: ");
            for (int i = 0; i < totalPages; i++)
            {
                sb.Append(string.Format("<a href='/Results.aspx?page={0}' class='{1}'>{0}</a>", i + 1, i + 1 == currentPage ? "current" : ""));
                sb.Append(" | ");
            }

            pager = sb.ToString();
        }
    }
}
