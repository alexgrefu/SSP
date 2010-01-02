using System.Collections.Generic;
using System.Linq;

namespace SkybrarySearch.Data
{
    public partial class Content
    {

        public bool HasImages
        {
            get { return Images.Count > 0; }
        }

        public bool HasNotes
        {
            get { return Notes.Count > 0; }
        }

        public bool HasTables
        {
            get { return Tables.Count > 0; }
        }


        public bool HasDefinitions
        {
            get { return ContentDefinitions.Count > 0; }
        }

        public string ToHTML()
        {
            if (!Notes.IsLoaded)
                Notes.Load();
            if (!Images.IsLoaded)
                Images.Load();
            if (!Tables.IsLoaded)
                Tables.Load();
            if (!ContentDefinitions.IsLoaded)
                ContentDefinitions.Load();

            var positionedElements = new List<IPositionedElement>();

            if (HasImages)
                foreach (var image in Images)
                    positionedElements.Add(image);

            if (HasNotes)
                foreach (var note in Notes)
                    positionedElements.Add(note);

            if (HasTables)
                foreach (var table in Tables)
                    positionedElements.Add(table);

            if (HasDefinitions)
            {
                foreach (var cd in ContentDefinitions)
                {
                    cd.Definition.PositionInContent = cd.PositionInContent;
                    positionedElements.Add(cd.Definition);
                }
            }


            var seed = 0;


            string formattedContent = "";
            if (Text.Length > 0)
            {
                string[] pieces = Text.Split('\n');
                for (int i = 0; i < pieces.Length - 1; i++)
                    formattedContent += SurroundText(pieces[i]);
            }


            foreach (var element in positionedElements.OrderBy(e => e.PositionInContent))
            {
                if (element is Image)
                {
                    string img = ((Image) element).ToHTML();
                    formattedContent = formattedContent.Insert(element.PositionInContent + seed, img);
                    // scad 1, deorece parserul adauga o pozitie la content cand gaseste o imagine
                    seed += (img.Length - 1);
                }
                else if (element is Table)
                {
                    string table = ((Table)element).ToHTML();
                    formattedContent = formattedContent.Insert(element.PositionInContent + seed, table);
                    // la fel ca la imagine
                    seed += (table.Length - 1);
                }
                else if (element is Note)
                {
                    string note = ((Note)element).ToHTML();
                    formattedContent = formattedContent.Insert(element.PositionInContent + seed, note);
                    seed += ((Note)element).AddedCharsToContent;
                }
                else if (element is Definition)
                {
                    string definition = ((Definition) element).ToHTML();
                    formattedContent = formattedContent.Insert(element.PositionInContent + seed, definition);
                    seed += (definition.Length - 1);
                    
                }
            }


            return formattedContent;
        }

        private static string SurroundText(string text)
        {
            if (text.Length > 0)
            {
                var format = text.Replace(text, "<p>" + text + "</p>");
                return format;
            }

            return "";

        }
    }
}