using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SkybrarySearch.Data
{
    public partial class Chapter
    {
        
        public bool HasContent()
        {
            bool hasContent = false;

            if (!ContentReference.IsLoaded)
                ContentReference.Load();
            if (Content.Text.Trim().Length > 0)
                hasContent = true;
            if (Content.HasNotes || Content.HasImages || Content.HasTables)
                hasContent = true;
            if (Content.HasDefinitions)
                hasContent = false;

            return hasContent;
        }

        public string ToSearchIndex()
        {
            if (!ContentReference.IsLoaded)
                ContentReference.Load();
            var sb = new StringBuilder();
            sb.Append(Title);

            return sb.ToString();
        }

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
            sb.Append(Title);
            sb.Append("</h3>");
            sb.Append("</div>");
            sb.Append("<div>");
            sb.Append(Content.ToHTML());
            sb.Append("</div>");

            return sb.ToString();
        }
    }
}
