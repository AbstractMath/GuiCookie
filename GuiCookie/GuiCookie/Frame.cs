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

        public Frame(FrameTemplate Template)
            : base(Template)
        {
            Elements = Template.Elements;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

        }

        public override void Update(GameTime gameTime)
        {

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

    }

    public class FrameTemplate : ElementTemplate
    {
        public List<Element> Elements;
    }
}
