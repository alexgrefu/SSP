using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Lucene.Net.Index;
using Lucene.Net.Search;

namespace SkybrarySearchPrototype
{
    public class SkybraryFilter : Filter
    {
        private string[] _documents;
        public SkybraryFilter(string[] documents)
        {
            _documents = documents;
        }
        public override BitArray Bits(IndexReader reader)
        {
            BitArray bits = new BitArray(reader.MaxDoc());

            return bits;
        }
    }
}
