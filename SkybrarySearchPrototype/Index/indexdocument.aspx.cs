﻿using System;
using SkybrarySearch.Index;

namespace SkybrarySearchPrototype
{
    public partial class IndexDocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("~/Index/Documents.aspx");
            else
                AddDocumentToLuceneIndex(Int32.Parse(Request.QueryString["id"]));       
        }

        private void AddDocumentToLuceneIndex(int documentId)
        {
            FSIndexer indexer = new FSIndexer();
            indexer.Index(documentId);
            Response.Redirect("~/Index/Documents.aspx");
        }
    }
}
