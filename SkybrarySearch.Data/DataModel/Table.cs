using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace SkybrarySearch.Data
{
    public partial class Table : IPositionedElement
    {

        public string ToSearchIndex()
        {

            if (!TableCells.IsLoaded)
                TableCells.Load();

            var sb = new StringBuilder();

            sb.Append(Name + "\n");

            var rows = from tc in TableCells
                       group tc by tc.Row
                           into r
                           select new { Cells = r };

            foreach (var row in rows)
            {
                foreach (var cell in row.Cells)
                {
                    sb.Append(cell.ToSearchIndex());
                    sb.Append("\t");
                }
                sb.Append("\n");
            }

            return sb.ToString();
        }
        
        public string ToHTML()
        {
            if (!TableCells.IsLoaded)
                TableCells.Load();

            var sb = new StringBuilder();

            sb.Append("<table cellpading='0' cellspacing='0' border='1'>");
            sb.Append("<caption>");
            sb.Append(Name);
            sb.Append("</caption>");
            sb.Append("<tbody>");

            var rows = from tc in TableCells
                       group tc by tc.Row
                           into r
                           select new { Cells = r };

            foreach (var row in rows)
            {
                sb.Append("<tr>");
                foreach (var cell in row.Cells)
                {
                    sb.Append("<td");

                    if (cell.RowSpan != 0 && cell.ColSpan != 0)
                        sb.Append(" rowspan='" + cell.RowSpan + "' colspan='" + cell.ColSpan + "'>");
                    else if (cell.RowSpan != 0 && cell.ColSpan == 0)
                        sb.Append(" rowspan='" + cell.RowSpan + "'>");
                    else if (cell.RowSpan == 0 && cell.ColSpan != 0)
                        sb.Append(" colspan='" + cell.ColSpan + "'>");
                    else
                        sb.Append(">");

                    sb.Append(cell.ToHTML());
                    sb.Append("</td>");
                }
                sb.Append("</tr>");    
            }

            sb.Append("</tbody>");
            sb.Append("</table>");

            return sb.ToString();

        }

        #region IPositionedElement Members


        public int Length
        {
            get { return ToHTML().Length; }
        }

        public int TextLenght
        {
            get { return ToSearchIndex().Length; }
        }

        #endregion
    }
}
