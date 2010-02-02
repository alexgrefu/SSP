using Lucene.Net.Index;

namespace SkybrarySearch.Index
{
    public interface ILuceneIndexer
    {
        IndexWriter Writer { get; }

        void Index(int documentId);
        void Delete(int documentId);
    }
}
