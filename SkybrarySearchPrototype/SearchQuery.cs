using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkybrarySearchPrototype
{
    public class SearchQuery
    {
        public string Query { get; set; }
        public List<string> Filters { get; set;}

        public string GetUserQuery()
        {
            return Query;
        }

        public string ToTooltip()
        {
            string text = "";
            text += "<h3>Filter by document(s):</h3>";

            text += "<ul>";
            foreach (var filter in Filters)
            {
                text += "<li>";
                text += filter;
                text += "</li>";
            }
            text += "</ul>";
            return text;
        }

        public string  GetComplexQuery()
        {
            var query = "";
            if (Filters != null)
            {
                if (Filters.Count > 1)
                    query += "(";
                for (int i = 0; i < Filters.Count; i++ )
                {
                    if (i == Filters.Count - 1)
                    {
                        query += "document:\"";
                        query += Filters[i];
                        query += "\" ";
                    }
                    else
                    {
                        query += "document:\"";
                        query += Filters[i];
                        query += "\" OR ";
                    }

                }
                if (Filters.Count > 1)
                    query += ") ";

                query += "AND " + string.Format("\"{0}\"", Query);

                return query;
            }

            return string.Format("\"{0}\"", Query);
        }

        public string GetQuery ()
        {
            return Query;
        }
    }
}
