using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using SkybrarySearch.Data;

namespace SkybrarySearchPrototype
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://www.jlg.ro/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ImageViewer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            int id = 0;
            if (context.Request.QueryString["id"] != null)
            {
                id = Int32.Parse(context.Request.QueryString["id"]);
            }
            using (var db = new SkybraryEntities())
            {
                var image = db.ImageSet.Where(i => i.Id == id).FirstOrDefault();

                if (image != null)
                {
                    context.Response.ContentType = "image/jpeg";
                    context.Response.OutputStream.Write(image.Data, 0, image.Data.Length);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
