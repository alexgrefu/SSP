using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkybrarySearch.Index;

namespace SkybrarySearchPrototype
{
    public partial class Index : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //throw new Exception("A test exception");

        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSearchQuery.Text.Trim().Length > 0)
            {
                HiddenField hf = cphMain.FindControl("txtActualFilter") as HiddenField;

                if (hf != null)
                {
                    var docs = hf.Value.Split('|');

                    if (docs.Length > 0)
                    {

                        List<string> f = new List<string>();

                        for (int i = 0; i < docs.Length; i++)
                            f.Add(docs[i]);

                        if (docs.Length == 1 && docs[0].Length == 0)
                            f = null;

                        Session["query"] = new SearchQuery { Filters = f, Query = txtSearchQuery.Text };
                    }
                    else
                    {
                        Session["query"] = new SearchQuery { Filters = null, Query = txtSearchQuery.Text };
                    }
                }

                var query = Session["query"] as SearchQuery;
                query.Query = txtSearchQuery.Text;

                Response.Redirect("Results.aspx");

            }
        }
    }
}
