using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SkybrarySearch.Data;
using Telerik.Web.UI;

namespace SkybrarySearchPrototype
{
    public partial class Documentview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request.QueryString["id"]);
            string type = Request.QueryString["type"];

            using (var db = new SkybraryEntities())
            {
                if (type == "paragraph")
                {
                    var paragraph = db.ParagraphSet.Where(p => p.Id == id).FirstOrDefault();

                    paragraph.ChapterReference.Load();
                    paragraph.Chapter.DocumentReference.Load();

                    var document = paragraph.Chapter.Document;


                    var chapters =
                        (from c in db.ChapterSet.Include("Paragraphs")
                         orderby c.OrderInDocument
                         where c.Document.Id == document.Id
                         select c).ToList();


                    BuildDocumentTreeView(chapters, document.Title, paragraph.Id, type);
                }
                else if (type == "chapter")
                {
                    var chapter = db.ChapterSet.Where(c => c.Id == id).FirstOrDefault();
                    chapter.DocumentReference.Load();
                    var document = chapter.Document;

                    var chapters =
                        (from c in db.ChapterSet.Include("Paragraphs")
                         orderby c.OrderInDocument
                         where c.Document.Id == document.Id
                         select c).ToList();

                    BuildDocumentTreeView(chapters, document.Title, chapter.Id, type);
                }
            }
        }

        public void BuildDocumentTreeView(List<Chapter> chapters, string documentTitle, int elementId, string type)
        {

            headerTitle.InnerText = "Document: " + documentTitle;

            foreach (var chapter in chapters)
            {
                var chapterNode = new RadTreeNode
                                              {
                                                  Text = chapter.Title, 
                                                  Value = chapter.Id.ToString()
                                              };
                chapterNode.Attributes.Add("t", "chapter");

                if (type == "chapter")
                {
                    if (elementId == chapter.Id)
                    {
                        //chapterNode.Expanded = true;
                        chapterNode.Selected = true;
                    }
                }

                foreach (var paragraph in chapter.Paragraphs.OrderBy(p => p.OrderInChapter))
                {
                    var paragraphNode = new RadTreeNode
                                            {
                                                Text = paragraph.Number + " " + paragraph.Title,
                                                Value = paragraph.Id.ToString()
                                            };
                    paragraphNode.Attributes.Add("t", "paragraph");

                    if (type == "paragraph")
                    {
                        if (elementId == paragraph.Id)
                        {
                            chapterNode.Expanded = true;
                            paragraphNode.Selected = true;
                        }
                    }

                    chapterNode.Nodes.Add(paragraphNode);
                }
                rtvDocument.Nodes.Add(chapterNode);
            }
            
        }
    }
}
