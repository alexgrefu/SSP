using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Store;
using System.Web;
using SkybrarySearch.Data;

namespace SkybrarySearch.Index
{
    public class FSIndexer : ILuceneIndexer
    {
        private IndexWriter _writer;
        private readonly Directory _directory;

        public FSIndexer() 
            : this(FSDirectory.GetDirectory(HttpContext.Current.Server.MapPath("~/LuceneIndex"))) {}

        public FSIndexer(Directory directory)
        {
            _directory = directory;
        }
            

        public IndexWriter Writer
        {
            get
            {
                if (_writer == null)
                    _writer = new IndexWriter(_directory, new StandardAnalyzer());
                
                return _writer; 
            }
        }

        public void Index(int documentId)
        {
            using (var db = new SkybraryEntities())
            {
                var icaoDocument = db.DocumentSet.Where(d => d.Id == documentId).First();

                var chapters =
                    (from c in db.ChapterSet
                         .Include("Content.ContentDefinitions.Definition.Content")
                         .Include("Paragraphs.Content.ContentDefinitions.Definition.Content")
                     where c.Document.Id == documentId
                     select c).ToList();

                foreach (var chapter in chapters)
                {
                    if (chapter.HasContent())
                    {
                        Field titleField = new Field("Title", chapter.Title,
                                                     Field.Store.YES, Field.Index.TOKENIZED,
                                                     Field.TermVector.WITH_POSITIONS_OFFSETS);
                        titleField.SetBoost(1.1F);
                        var doc = new Lucene.Net.Documents.Document();
                        doc.Add(new Field("IcaoId", icaoDocument.Id.ToString(), Field.Store.YES,
                                          Field.Index.UN_TOKENIZED));
                        doc.Add(new Field("DocumentTitle", icaoDocument.Title, Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("Type", "chapter", Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("Id", chapter.Id.ToString(), Field.Store.YES, Field.Index.NO));
                        doc.Add(new Field("OrderInDocument", chapter.OrderInDocument.ToString(), Field.Store.YES,
                                          Field.Index.NO));
                        doc.Add(titleField);
                        doc.Add(new Field("Content", chapter.Content.ToSearchIndex(),
                                          Field.Store.YES, Field.Index.TOKENIZED,
                                          Field.TermVector.WITH_POSITIONS_OFFSETS));
                        Writer.AddDocument(doc);

                    }

                    foreach (var paragraph in chapter.Paragraphs)
                    {
                        if (paragraph.HasContent())
                        {
                            Field titleField = new Field("Title", paragraph.Number + " " + paragraph.Title,
                                                         Field.Store.YES,
                                                         Field.Index.TOKENIZED,
                                                         Field.TermVector.WITH_POSITIONS_OFFSETS);
                            titleField.SetBoost(1.1F);
                            var par = new Lucene.Net.Documents.Document();
                            par.Add(new Field("IcaoId", icaoDocument.Id.ToString(), Field.Store.YES,
                                              Field.Index.UN_TOKENIZED));
                            par.Add(new Field("DocumentTitle", icaoDocument.Title, Field.Store.YES, Field.Index.NO));
                            par.Add(new Field("Type", "paragraph", Field.Store.YES, Field.Index.NO));
                            par.Add(new Field("Id", paragraph.Id.ToString(), Field.Store.YES, Field.Index.NO));
                            par.Add(new Field("Chapter", chapter.Title, Field.Store.YES, Field.Index.NO));
                            par.Add(new Field("OrderInChapter", paragraph.OrderInChapter.ToString(), Field.Store.YES,
                                              Field.Index.NO));
                            par.Add(titleField);
                            par.Add(new Field("Content", paragraph.Content.ToSearchIndex(),
                                              Field.Store.YES, Field.Index.TOKENIZED,
                                              Field.TermVector.WITH_POSITIONS_OFFSETS));
                            Writer.AddDocument(par);
                        }
                    }
                }

                Writer.Optimize();
                Writer.Flush();
                Writer.Close();
            }
        }

        public void Delete(int documentId)
        {
            Writer.DeleteDocuments(new Term("IcaoId", documentId.ToString()));
            Writer.Optimize();
            Writer.Flush();
            Writer.Close();
        }
    }
}
