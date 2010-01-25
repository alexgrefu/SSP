using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Highlight;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using SkybrarySearch.Data;
using Directory=Lucene.Net.Store.Directory;
using Lucene.Net.Index;

namespace SkybrarySearchPrototype
{
    public partial class Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["query"] == null)
                Response.Redirect("~/skysearch.aspx");

            var query = Session["query"] as SearchQuery;

            if (!Page.IsPostBack)
            {
                ((TextBox)Master.FindControl("txtSearchQuery")).Text = query.GetUserQuery();

                DoSearch(query.GetComplexQuery(), GetSearchTerms(query.GetUserQuery()));

                if (query.Filters != null)
                {
                    filterText.InnerHtml = "<span><b>The query is filtered!</b></span> <a id='spanTooltip' href='javascript:alert(\"Not implemented!\")'>[remove filters]</a>";
                    tooltipItem.InnerHtml = query.ToTooltip();
                }
            }

        }

        public WeightedTerm[] GetSearchTerms(string searchTxt)
        {
            string[] arr = searchTxt.Trim().Split(' ');

            WeightedTerm[] terms = new WeightedTerm[arr.Length];

            for (int i = 0; i < arr.Length; i++)
                terms[i] = new WeightedTerm(1.0f, arr[i]);

            return terms;
        }

        public void DoSearch(string searchText, WeightedTerm[] terms)
        {
            Directory directory = FSDirectory.GetDirectory(Server.MapPath("~/LuceneIndex"));
            Analyzer analyzer = new StandardAnalyzer();
            //QueryParser parser = new QueryParser("content", analyzer); 
            //QueryParser parser = new QueryParser("content", analyzer);

            QueryParser parser = new MultiFieldQueryParser(new[] {"title", "content"}, new StandardAnalyzer());
            Query query = parser.Parse(searchText);


            //Setup searcher
            IndexSearcher searcher = new IndexSearcher(directory);
            //Do the search

            

            Hits hits = searcher.Search(query);
            
            int results = hits.Length();

           

            List<LuceneDocument> docs = new List<LuceneDocument>();

            for (int i = 0; i < results; i++)
            {
                var doc = hits.Doc(i);

                docs.Add(new LuceneDocument
                {
                    Id = Int32.Parse(doc.Get("id")),
                    Type = doc.Get("type"),
                    Content = GeneratePreviewText(terms, doc.Get("content")),
                    Document = doc.Get("document"),
                    Chapter = doc.Get("chapter"),
                    Title = HighlightTitle(terms, doc.Get("title")),
                    OrderInDocument = Int32.Parse(doc.Get("orderInDocument")),
                    OrderInChapter = Int32.Parse(doc.Get("orderInChapter"))
                });

            }

            
            searcher.Close();

            var documents = from d in docs
                            group d by d.Document
                                into h
                                select new
                                           {
                                               Hits = (from i in h select i)
                                                        .OrderBy(i => i.OrderInDocument)
                                                        .OrderBy(i => i.OrderInChapter), 
                                               Count = h.Count(), 
                                               Title = h.Key
                                           };

            infoHeader.InnerText = string.Format("Query returned {0} results (exact matches) found in {1} document(s)", hits.Length(), documents.Count());

            /*
            lvSearchResults.DataSource = documents;
            lvSearchResults.DataBind();
            */

            accSearchResults.DataSource = documents;
            accSearchResults.DataBind();

            //rpbSearchResults.DataSource = documents;
            //rpbSearchResults.DataBind();
        }

        public string HighlightTitle(WeightedTerm[] terms, string text)
        {
            QueryScorer scorer = new QueryScorer(terms);

            
            Formatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new NullFragmenter());
            TokenStream stream = new StandardAnalyzer().TokenStream("title", new StringReader(text));
            string preview = highlighter.GetBestFragment(stream, text);
            if (preview != null)
                return preview;

            return text;
        }

        public string GeneratePreviewText(WeightedTerm[] terms, string text)
        {
            QueryScorer scorer = new QueryScorer(terms);
            Formatter formatter = new SimpleHTMLFormatter("<span style='color:maroon; font-weight:bold;'>", "</span>");
            Highlighter highlighter = new Highlighter(formatter, scorer);
            highlighter.SetTextFragmenter(new SimpleFragmenter(150));
            TokenStream stream = new StandardAnalyzer().TokenStream("content", new StringReader(text));
            string preview = highlighter.GetBestFragment(stream, text);
            if (preview != null)
                return StripParagraphs(preview);
         
            return text;
            
        }

        public string StripParagraphs(string text)
        {
            if (text.LastIndexOf("</p>") != -1)
            {
                if (text.LastIndexOf("</p>") < text.Length - "</p>".Length)
                    text = text.Remove(text.LastIndexOf("</p>") + "</p>".Length);
            }


            if (text.Contains("<em>") && text.Contains("</em>"))
            {
                if (text.IndexOf("<em>") > text.IndexOf("</em>"))
                    text = "<em>" + text + "</em>";
                
                    
            }
            if (text.Contains("<em>") && !text.Contains("</em>"))
            {
                if (text.EndsWith("</p>"))
                    text.Insert(text.IndexOf("</p>"), "</em>");
                
                text = text + "</em>";
            }
            if (!text.Contains("<em>") && text.Contains("</em>"))
            {
                if (text.StartsWith("<p>"))
                    text.Insert("<p>".Length, "<em>");
                if (text.LastIndexOf("<em>") > text.LastIndexOf("</em>"))
                    text += "</em>";
            }
            if (text.EndsWith("<em"))
                text = text.Replace("<em", "");
            if (text.StartsWith("<p>"))
                return text + " ...";
            if (text.EndsWith("</p>"))
                return "... " + text;
            else
            {
                return "... " + text + " ...";
            }
        }
    }
}
