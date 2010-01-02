using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SkybrarySearch.Data
{
    public partial class Note : IPositionedElement
    {
        public int AddedCharsToContent
        {
            get
            {
                return ToHTML().Length - Text.Length;
            }
        }


        public string ToHTML()
        {
            var sb = new StringBuilder();
            sb.Append("<p><em>");
            sb.Append(Text);
            sb.Append("</em></p>");

            return sb.ToString();
        }


        #region IPositionedElement Members


        public int Length
        {
            get { return ToHTML().Length; }
        }

        #endregion
    }
}
