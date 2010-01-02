using System.Text;

namespace SkybrarySearch.Data
{
    public partial class Image : IPositionedElement
    {
        public string ToHTML()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append("<h4>");
            sb.Append(Name);
            sb.Append("</h4>");
            sb.Append("<p><img src='");
            sb.Append("/ImageViewer.ashx?id=" + Id);
            sb.Append("' width='");
            sb.Append(Width.ToString());
            sb.Append("' height='");
            sb.Append(Height.ToString());
            sb.Append("' alt='");
            sb.Append(Name);
            sb.Append("' /></p>");

            return sb.ToString();
        }

        public int Length
        {
            get
            {
                return ToHTML().Length;
            }
        }
    }
}
