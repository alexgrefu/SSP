using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkybrarySearch.Data
{
    public partial class TableCell
    {
        public string ToHTML()
        {
            if (!ContentReference.IsLoaded)
                ContentReference.Load();
            
            return Content.ToHTML();
        }

        public string ToSearchIndex()
        {
            if (!ContentReference.IsLoaded)
                ContentReference.Load();

            return Content.ToSearchIndex();
        }
    }
}
