using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using SkybrarySearch.Data;

namespace SkybrarySearchPrototype
{
    public partial class view : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("default.aspx");

            var id = Int32.Parse(Request.QueryString["id"]);
            var elementType = Request.QueryString["type"].ToString();

            using (var db = new SkybraryEntities())
            {
                if (elementType == "paragraph")
                {
                    var paragraph = db.ParagraphSet
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")
                        .Where(p => p.Id == id).First();

                    htmlContainer.InnerHtml = paragraph.ToHTML();
                }
                else if (elementType == "chapter")
                {
                    var chapter = db.ChapterSet
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")
                        .Where(c => c.Id == id).First();

                    htmlContainer.InnerHtml = chapter.ToHTML();
                }
            }

        }
    }
}
