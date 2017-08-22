using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace GuiCookie.Elements
{
    public class LineGraph : Element
    {
        private Interval interval;
        private Line line;
        private Color[] colourData { get; set; }
        private Texture2D graphTexture { get; set; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //Draw the normal texture first, then the graph texture over it
        }

        public LineGraph(XmlAttributeCollection attributes, IGuiContainer parent)
            :base(attributes, parent)
        {

        }

        private class Interval
        {
            public Color Colour { get; set; }
            public int Space { get; set; }
            public int HideUnder { get; set; }
        }

        private class Line
        {
            public Color Colour { get; set; }
            public Color Fill { get; set; }
            public int HeadRoom { get; set; }
            public bool DoFill { get; set; }
        }
    }
}
