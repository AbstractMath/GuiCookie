using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiCookie
{
    public class Frame : Element, IGuiContainer
    {

        List<Element> Elements;

        public Frame(ElementTemplate Template)
            : base(Template)
        {
            Elements = new List<Element>();
        }

        public override void Draw(SpriteBatch SpriteBatch)
        {
            //If the element is invisible, don't bother drawing
            if (!Visible) return;
            foreach (Element e in Elements)
            {
                e.Draw(SpriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (Element e in Elements)
            {
                e.Update(gameTime);
            }
        }

        public Element GetNamedElement(string Name)
        {
            foreach (Element e in Elements)
            {
                if (e.Name == Name)
                    return e;
            }
            return null;
        }

        public void AddElement(Element Element)
        {
            Elements.Add(Element);
        }
    }
}
