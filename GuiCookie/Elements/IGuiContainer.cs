using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GuiCookie.Elements
{
    public interface IGuiContainer
    {
        Rectangle ContentBounds { get; }
        List<Element> Elements { get; }
        Element GetElementByID(string id);
        void AddElement(Element element);
        void AddElements(List<Element> elements);
    }
}
