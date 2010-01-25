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
    public partial class documents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (var db = new SkybraryEntities())
            {

                var gd = (from doc in db.DocumentSet
                          group doc by doc.GroupName
                          into gc
                              select new
                                         {
                                             Documents = (from g in gc select g).OrderBy(g => g.OrderInGroup),
                                             Count = gc.Count(),
                                             Title = gc.Key,
                                             GroupOrder = (from g in gc select g.OrderInGroup).Min()
                                        });

                var list = gd.ToList().OrderBy(d => d.GroupOrder);

                StringBuilder sb = new StringBuilder();

                sb.Append("<ul>");
                foreach (var documentsGroup in list)
                {
                    sb.Append("<li>");
                    sb.Append(documentsGroup.Title);
                    sb.Append("<ul>");
                    
                    var gds = from g in documentsGroup.Documents
                              group g by g.ShortName
                              into gc
                                  select new
                                             {
                                                 SubDocuments = gc, 
                                                 Title = gc.Key, 
                                                 Count = gc.Count(),
                                                 Id = gc.Count() == 1 ? gc.Where(d => d.Title == gc.Key).Select(i => i.Id).Single() : -1
                                             };
                    foreach (var subdocument in gds)
                    {
                        sb.Append("<li>");
                        if (subdocument.Id != -1)
                            sb.Append(string.Format("{0} <a href='viewdocument.aspx?id={1}'>[view]</a> <a href='indexdocument.aspx?id={1}'>[index]</a>", subdocument.Title, subdocument.Id));
                        else
                            sb.Append(subdocument.Title);
                        if (subdocument.Count > 1)
                        {
                            sb.Append("<ul>");
                            foreach (var sd in subdocument.SubDocuments)
                            {
                                sb.Append("<li>");
                                sb.Append(string.Format("{0} <a href='viewdocument.aspx?id={1}'>[view]</a> <a href='indexdocument.aspx?id={1}'>[index]</a>", sd.Title, sd.Id));
                                sb.Append("</li>");
                            }
                            sb.Append("</ul>");
                                
                        }
                        sb.Append("</li>");
                    }
                    
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                sb.Append("</ul>");

                htmlContainer.InnerHtml = sb.ToString();

            }
        }
    }
}
