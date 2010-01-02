﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SkybrarySearch.Data
{
    public partial class Definition : IPositionedElement
    {
        public string ToHTML()
        {
            if (!ContentReference.IsLoaded)
                ContentReference.Load();
            var sb = new StringBuilder();
            sb.Append("<h4>");
            sb.Append(Term);
            sb.Append("</h4>");
            sb.Append(Content.ToHTML());
            return sb.ToString();
        }

        #region IPositionedElement Members

        public int PositionInContent { get; set; }

        public int Length
        {
            get { return ToHTML().Length; }
        }

        #endregion
    }
}
