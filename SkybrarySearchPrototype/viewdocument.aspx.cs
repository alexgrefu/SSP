using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkybrarySearch.Data;

namespace SkybrarySearchPrototype
{
    public partial class viewdocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("documents.aspx");

            var documentId = Int32.Parse(Request.QueryString["id"]);
            var sb = new StringBuilder();
            using (var db = new SkybraryEntities())
            {
                var chapters =
                    (from c in db.ChapterSet.Include("Paragraphs")
                     where c.Document.Id == documentId
                     select c).ToList();
                sb.Append("<ul>");
                foreach (var chapter in chapters)
                {
                    sb.Append("<li>");
                    sb.Append("<a href='view.aspx?type=chapter&id=" + chapter.Id + "'>" + chapter.Title + "</a>");
                    if (chapter.Paragraphs.Count > 0)
                    {
                        sb.Append("<ul>");
                        foreach (var paragraph in chapter.Paragraphs.OrderBy(p => p.OrderInChapter))
                        {
                            sb.Append("<li>");
                            sb.Append("<a href='view.aspx?type=paragraph&id=" + paragraph.Id + "'>" + paragraph.Number + " " + paragraph.Title + "</a>");

                            sb.Append("</li>");
                        }
                        sb.Append("</ul>");
                    }
                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            htmlContainer.InnerHtml = sb.ToString();
        }
    }
}
