using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GuiCookie
{
    public class TextBlock : Element
    {
        private string text;
        public string Text { private get { return text; } set { text = value; } }
        private BoxStyle TextBlockStyle;
        private Texture2D TextBlockTexture;

        public TextBlock(TextBlockTemplate Template)
            : base(Template)
        {
            Text = Template.Text;
            TextBlockStyle = Template.TextBlockStyle;
            TextBlockTexture = GuiFunctions.ConstructElementTexture(TextBlockStyle, Size);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextBlockTexture, Position);
            spriteBatch.DrawString(TextBlockStyle.Font, Text, new Vector2(Position.X + ((Size.X - TextBlockStyle.Font.MeasureString(Text).X)) / 2, Position.Y + ((Size.Y - TextBlockStyle.Font.MeasureString(Text).Y) / 2)), Color.Black);
        }

    }

    public class TextBlockTemplate : ElementTemplate
    {
        public string Text;
        public BoxStyle TextBlockStyle;
    }
}
