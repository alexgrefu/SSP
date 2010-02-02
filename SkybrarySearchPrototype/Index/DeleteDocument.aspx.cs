using System;
using SkybrarySearch.Index;

namespace SkybrarySearchPrototype
{
    public partial class DeleteDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("~/Index/Documents.aspx");
            else
                DeleteDocumentFromIndex(Int32.Parse(Request.QueryString["id"]));   
        }

        private void DeleteDocumentFromIndex(int documentId)
        {
            FSIndexer indexer = new FSIndexer();
            indexer.Delete(documentId);
            Response.Redirect("~/Index/Documents.aspx");
        }
    }
}
