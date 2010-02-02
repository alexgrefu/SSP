using System;
using System.Linq;
using System.Text;
using SkybrarySearch.Data;
using SkybrarySearch.Index;

namespace SkybrarySearchPrototype
{
    public partial class Documents : System.Web.UI.Page
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
                        {

                            int id = subdocument.Id;
                            
                            sb.Append(
                                string.Format(
                                    "{0} {1} {2} {3}", subdocument.Title, ViewLink(id), IndexLink(id), DeleteLink(id)));
                        }
                        else
                            sb.Append(subdocument.Title);
                        if (subdocument.Count > 1)
                        {
                            sb.Append("<ul>");
                            foreach (var sd in subdocument.SubDocuments)
                            {
                                int id = sd.Id;
                                sb.Append("<li>");
                                sb.Append(
                                string.Format(
                                    "{0} {1} {2} {3}", sd.Title, ViewLink(id), IndexLink(id), DeleteLink(id)));
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

        public string DeleteLink(int id)
        {
            LuceneSearcher searcher = new LuceneSearcher(new SearchQuery());
           
            return !searcher.DocIsIndexed(id)
                ?
                string.Empty
                :
                string.Format("<a href='deletedocument.aspx?id={0}'>[delete]</a>", id);

        }

        public string IndexLink(int id)
        {
            LuceneSearcher searcher = new LuceneSearcher(new SearchQuery());

            return searcher.DocIsIndexed(id)
                ? 
                string.Empty 
                : 
                string.Format("<a href='indexdocument.aspx?id={0}'>[index]</a>", id);
        }

        public string ViewLink(int id)
        {
            return string.Format("<a href='viewdocument.aspx?id={0}'>[view]</a>", id);
        }
    }
}
