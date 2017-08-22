using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiCookie.StyleStructures
{
    public struct ElementStyle
    {
        public Type Type { get; set; }
        public string Name { get; set; }

        public Dictionary<string, StyleParameter> Parameters { get; set; }
    }
}
