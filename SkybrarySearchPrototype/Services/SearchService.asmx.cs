using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SkybrarySearch.Data;


namespace SkybrarySearchPrototype.Services
{
    /// <summary>
    /// Summary description for SearchService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SearchService : System.Web.Services.WebService
    {

        [WebMethod]
        public object GetTextElementContent(int id, string type)
        {
            string content = "";

            using (var db = new SkybraryEntities())
            {
                if (type == "paragraph")
                {
                    var paragraph = db.ParagraphSet
                        .Include("Chapter")
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")
                        .Include("Content.Notes")
                        .Include("Content.Tables")
                        .Include("Content.Tables.TableCells")
                        .Include("Content.Images")
                        .Where(p => p.Id == id).First();

                    content = "<p><b>" + paragraph.Chapter.Title + "</b></p>" + paragraph.ToHTML();

                    var chapter = paragraph.Chapter;
                    chapter.Paragraphs.Load();

                    int nextId = 0;
                    int prevId = 0;
                    string nextType = "";
                    string prevType = "";

                    var nextParagraph = (from p in chapter.Paragraphs
                                         where p.OrderInChapter > paragraph.OrderInChapter
                                         orderby p.OrderInChapter ascending 
                                         select p).FirstOrDefault();
                    var prevParagraph = (from p in chapter.Paragraphs
                                         where p.OrderInChapter < paragraph.OrderInChapter
                                         orderby p.OrderInChapter descending
                                         select p).FirstOrDefault();
                   


                    if (nextParagraph != null)
                    {
                        nextId = nextParagraph.Id;
                        nextType = "paragraph";
                    }
                    else
                    {
                        nextId = paragraph.Id;
                        nextType = "paragraph";
                    }
                    if (prevParagraph != null)
                    {
                        prevId = prevParagraph.Id;
                        prevType = "paragraph";
                    }
                    else
                    {
                        prevId = paragraph.Id;
                        prevType = "paragraph";
                    }

                    content += string.Format(@"
                                    <hr /> 
                                    <table style='width:100%'>
                                        <tr>
                                            <td align='right'>
                                                <a href='javascript:ShowNext({0}, ""{1}"")'>Previous Paragraph</a> | <a href='javascript:ShowNext({2}, ""{3}"")'>Next Paragraph</a>
                                            </td>
                                        </tr>
                                    </table>", prevId, prevType, nextId, nextType);
                }
                else if (type == "chapter")
                {
                    var chapter = db.ChapterSet
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")
                        .Include("Content.Notes")
                        .Include("Content.Tables")
                        .Include("Content.Tables.TableCells")
                        .Include("Content.Images")
                        .Where(c => c.Id == id).First();

                    content = chapter.ToHTML();
                }
                
            }

            return new {NextElement = 1, Content = content};
        }


        [WebMethod]
        public object GetTextElementForDocument(int id, string type)
        {
            string content = "";

            using (var db = new SkybraryEntities())
            {
                if (type == "paragraph")
                {
                    var paragraph = db.ParagraphSet
                        .Include("Chapter")
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")                        
                        .Where(p => p.Id == id).First();

                    content = paragraph.ToHTML();
                }
                else if (type == "chapter")
                {
                    var chapter = db.ChapterSet
                        .Include("Content")
                        .Include("Content.ContentDefinitions")
                        .Include("Content.ContentDefinitions.Definition")
                        .Where(c => c.Id == id).First();

                    content = chapter.ToHTML();
                }

            }

            return new { NextElement = 1, Content = content };
        }
    }
}
