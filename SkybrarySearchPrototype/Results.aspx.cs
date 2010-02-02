using System;
using System.Web.UI.WebControls;
using SkybrarySearch.Index;

namespace SkybrarySearchPrototype
{
    public partial class Results : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["query"] == null)
                Response.Redirect("~/Search.aspx");

            var query = Session["query"] as SearchQuery;

            if (!Page.IsPostBack)
            {
                ((TextBox)Master.FindControl("txtSearchQuery")).Text = query.Query;
                int currentPage = Request.QueryString["page"] != null ? Int32.Parse(Request.QueryString["page"]) : 1;
                DoSearch(query, currentPage);
            }
        }

        private void DoSearch(SearchQuery query, int currentPage)
        {
            LuceneSearcher searcher = new LuceneSearcher(query);
            
            var docs = searcher.DoSearch(10, currentPage);

            pager.InnerHtml = searcher.Pager;

            lvSearchResults.DataSource = docs;
            lvSearchResults.DataBind();
        }

        #region Commented Code
        //public void DoSearch(SearchQuery query)
        //{
            

        //    //GeneratePagerLinks(results, currentPage, pageSize);



        //    //var dox = from d in docs
        //    //                group d by d.Document
        //    //                    into h
        //    //                    select new
        //    //                   {
        //    //                       Hits = (from i in h select i)
        //    //                                .OrderBy(i => i.OrderInDocument)
        //    //                                .OrderBy(i => i.OrderInChapter),
        //    //                       Count = h.Count(),
        //    //                       Title = h.Key
        //    //                   };

        //    var time2 = TimeSpan.FromTicks(DateTime.Now.Ticks);

        //    TimeSpan execTime = time2 - time1;
        //    string time = execTime.Seconds + "." + execTime.Milliseconds;

        //    infoHeader.InnerText = string.Format("Query returned {0} results. The query executed in aproximately {1} seconds", results, time);

            
        //    lvSearchResults.DataSource = docs;
        //    lvSearchResults.DataBind();
            

        //    //accSearchResults.DataSource = documents;
        //    //accSearchResults.DataBind();

        //    //rpbSearchResults.DataSource = documents;
        //    //rpbSearchResults.DataBind();


        //}

        #endregion
    }
}
