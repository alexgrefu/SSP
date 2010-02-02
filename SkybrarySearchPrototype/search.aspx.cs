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
    public partial class Search : System.Web.UI.Page
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
                var gd = (from document in db.DocumentSet
                          group document by document.GroupName
                          into groupedDocuments
                          select new
                             {
                                 Documents = 
                                    (from groupedDocument in groupedDocuments 
                                     select groupedDocument).OrderBy(g => g.OrderInGroup),
                                 Count = groupedDocuments.Count(),
                                 Title = groupedDocuments.Key,
                                 GroupOrder =
                                    (from groupedDocument in groupedDocuments 
                                     select groupedDocument.OrderInGroup).Min()
                             });

                // grouped documents list
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
                        groupNode.Attributes["filter"] = documentsGroup.Documents.FirstOrDefault().Id.ToString();
                        groupNode.ToolTip = documentsGroup.Title;
                        rtvDocumentIndex.Nodes.Add(groupNode);
                    }
                    else
                    {
                        var gds = from groupedDocuments in documentsGroup.Documents
                                  group groupedDocuments by groupedDocuments.ShortName
                                  into gc
                                  select new
                                  {
                                     SubDocuments = gc,
                                     Title = gc.Key,
                                     Count = gc.Count(),
                                     Id = gc.Count() == 1 
                                                ? 
                                                gc.Where(d => d.Title == gc.Key)
                                                  .Select(i => i.Id)
                                                  .Single() 
                                                : 
                                                -1
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
                                    documentNode.Attributes["filter"] += sd.Id + ";";
                            }
                            else
                                documentNode.Attributes["filter"] = subdocument.Id.ToString();

                            groupNode.Nodes.Add(documentNode);
                            documentNode.ToolTip = subdocument.Title;
                        }

                        rtvDocumentIndex.Nodes.Add(groupNode);
                        groupNode.Expanded = true;
                    }
                }
            }
        }
    }
}
