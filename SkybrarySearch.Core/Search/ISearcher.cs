using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkybrarySearch.Data;

namespace SkybrarySearch.Index
{
    public interface ISearcher
    {
        IList<ViewDocument> DoSearch(int pageSize, int currentPage);
        bool DocIsIndexed(int documentId);

    }
}
