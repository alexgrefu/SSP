using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkybrarySearch.Data
{
    public class LuceneDocument
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Document { get; set; }
        public string Chapter { get; set; }
        public string Content { get; set; }
        public int OrderInDocument { get; set; }
        public int OrderInChapter { get; set; }
    }
}
