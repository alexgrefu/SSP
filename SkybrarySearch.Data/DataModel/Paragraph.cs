using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SkybrarySearch.Data
{
    public partial class Paragraph
    {
        public string ToHTML()
        {
            if (!ContentReference.IsLoaded)
                ContentReference.Load();

            var sb = new StringBuilder();

            sb.Append("<div>");
            sb.Append("<p>");
            sb.Append("Page: ");
            sb.Append(Page);
            sb.Append("</p>");
            sb.Append("<h3>");
            sb.Append(Number + " " + Title);
            sb.Append("</h3>");
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append(Content.ToHTML());
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}
