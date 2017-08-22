using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Xml;

namespace GuiCookie.Elements
{
    public class Frame : Element, IGuiContainer
    {
        public List<Element> Elements { get; private set; }


        public Frame(XmlAttributeCollection attributes, IGuiContainer parent)
            :base(attributes, parent)
        {
            Elements = new List<Element>();
        }

        public override void RecalculateStyle(Style style)
        {
            base.RecalculateStyle(style);

            foreach (Element e in Elements)
                e.RecalculateStyle(style);
            
        }

        /// <summary> Add a single element to this container's elements. </summary>
        /// <param name="element"> The element to add. </param>
        public void AddElement(Element element)
        {
            Elements.Add(element);
        }

        /// <summary> Add a list of elements to this container's elements. </summary>
        /// <param name="elements"> The list of elements to add. </param>
        public void AddElements(List<Element> elements)
        {
            Elements.AddRange(elements);
        }

        public Element GetElementByID(string id)
        {
            foreach (Element e in Elements)
            {
                if (e.ID == id)
                    return e;

                if (e is IGuiContainer g)
                {
                    Element eg = g.GetElementByID(id);
                    if (eg != null) return eg;
                }
            }

            return null;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Element e in Elements)
                e.Draw(spriteBatch);
        }

        public override void Update()
        {
            base.Update();

            foreach (Element e in Elements)
                e.Update();
        }
    }
}
