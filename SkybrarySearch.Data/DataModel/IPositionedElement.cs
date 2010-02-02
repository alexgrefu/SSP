using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkybrarySearch.Data
{
    public interface IPositionedElement
    {
        int PositionInContent { get; set; }
        int Length { get; }
        int TextLenght { get;  }
    }
}
