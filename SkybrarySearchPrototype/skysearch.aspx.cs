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
    public partial class skysearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BuildDocucumentIndex();
            }
        }

        private void BuildDocucumentIndex()
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


                foreach (var documentsGroup in list)
                {
                    RadTreeNode groupNode = new RadTreeNode
                                                {
                                                    Text = documentsGroup.Title,
                                                    Value = documentsGroup.Count.ToString()
                                                    
                                                };

                    if (documentsGroup.Title == "Convention on International Civil Aviation")
                    {
                        groupNode.Attributes["filter"] = documentsGroup.Title;
                        groupNode.ToolTip = documentsGroup.Title;
                        rtvDocumentIndex.Nodes.Add(groupNode);
                    }
                    else
                    {
                        var gds = from g in documentsGroup.Documents
                                  group g by g.ShortName
                                  into gc
                                      select new
                                                 {
                                                     SubDocuments = gc,
                                                     Title = gc.Key,
                                                     Count = gc.Count(),
                                                     Id =
                                      gc.Count() == 1 ? gc.Where(d => d.Title == gc.Key).Select(i => i.Id).Single() : -1
                                                 };

                        foreach (var subdocument in gds)
                        {
                            RadTreeNode documentNode = new RadTreeNode
                                                           {
                                                               Text = subdocument.Title,
                                                               Value = subdocument.Id.ToString()
                                                           };

                            if (subdocument.Count > 1)
                            {
                                foreach (var sd in subdocument.SubDocuments)
                                    documentNode.Attributes["filter"] += sd.Title + ";";
                            }
                            else
                                documentNode.Attributes["filter"] = subdocument.Title;

                            groupNode.Nodes.Add(documentNode);
                            documentNode.ToolTip = documentNode.Attributes["filter"];
                        }

                        rtvDocumentIndex.Nodes.Add(groupNode);
                        groupNode.Expanded = true;
                    }
                }
            }
        }
    }
}
