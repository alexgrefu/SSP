using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Lucene.Net;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Highlight;
using SkybrarySearch.Data;
using Directory=Lucene.Net.Store.Directory;

namespace SkybrarySearchPrototype
{
    public partial class indexdocument : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] == null)
                Response.Redirect("documents.aspx");

            int id = Int32.Parse(Request.QueryString["id"]);

            using (var db = new SkybraryEntities())
            {
                var icaoDocument = db.DocumentSet.Where(d => d.Id == id).First();

                var docTitle = icaoDocument.Title;
                var chapters =
                    (from c in db.ChapterSet
                         .Include("Content")
                         .Include("Content.ContentDefinitions")
                         .Include("Content.ContentDefinitions.Definition")
                         .Include("Paragraphs")
                         .Include("Paragraphs.Content")
                         .Include("Paragraphs.Content.ContentDefinitions")
                         .Include("Paragraphs.Content.ContentDefinitions.Definition")
                     where c.Document.Id == id
                     select c).ToList();
                
                Directory directory = FSDirectory.GetDirectory(Server.MapPath("~/LuceneIndex"));
                Analyzer analyzer = new StandardAnalyzer();
                IndexWriter writer = new IndexWriter(directory, analyzer);

                foreach (var chapter in chapters)
                {
                    Lucene.Net.Documents.Document c = new Lucene.Net.Documents.Document();
                    c.Add(new Field("type", "chapter", Field.Store.YES, Field.Index.NO));
                    c.Add(new Field("id", chapter.Id.ToString(), Field.Store.YES, Field.Index.NO));
                    c.Add(new Field("documentId", icaoDocument.Id.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
                    c.Add(new Field("document", docTitle, Field.Store.YES, Field.Index.TOKENIZED));
                    c.Add(new Field("chapter", "none", Field.Store.YES, Field.Index.NO));
                    c.Add(new Field("orderInDocument", chapter.OrderInDocument.ToString(), Field.Store.YES, Field.Index.NO));
                    c.Add(new Field("orderInChapter", "0", Field.Store.YES, Field.Index.NO));
                    c.Add(new Field("title", chapter.Title, Field.Store.YES, Field.Index.TOKENIZED));
                    c.Add(new Field("content", chapter.Content.ToHTML(), Field.Store.YES, Field.Index.TOKENIZED));
                    writer.AddDocument(c);

                    foreach (var paragraph in chapter.Paragraphs)
                    {
                        Lucene.Net.Documents.Document p = new Lucene.Net.Documents.Document();
                        p.Add(new Field("type", "paragraph", Field.Store.YES, Field.Index.NO));
                        p.Add(new Field("id", paragraph.Id.ToString(), Field.Store.YES, Field.Index.NO));
                        c.Add(new Field("documentId", icaoDocument.Id.ToString(), Field.Store.YES, Field.Index.UN_TOKENIZED));
                        p.Add(new Field("document", docTitle, Field.Store.YES, Field.Index.TOKENIZED));
                        p.Add(new Field("chapter", chapter.Title, Field.Store.YES, Field.Index.NO));
                        p.Add(new Field("orderInDocument", chapter.OrderInDocument.ToString(), Field.Store.YES, Field.Index.NO));
                        p.Add(new Field("orderInChapter", paragraph.OrderInChapter.ToString(), Field.Store.YES, Field.Index.NO));
                        p.Add(new Field("title", paragraph.Number + " " + paragraph.Title, Field.Store.YES, Field.Index.TOKENIZED));
                        p.Add(new Field("content", paragraph.Content.ToHTML(), Field.Store.YES, Field.Index.TOKENIZED));
                        writer.AddDocument(p);
                    }
                }

                writer.Optimize();
                writer.Flush();
                writer.Close();
               
            }

            

        }

        
    }
}
