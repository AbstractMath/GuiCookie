using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml;

namespace GuiCookie.Elements
{
    public class TextBlock : Element
    {
        #region PRIVATE PROPERTIES
        protected SpriteFont font;
        protected Vector2 textAnchor;
        #endregion

        #region PUBLIC PROPERTIES
        public string Text { get; set; }
        public Color TextColour { get; set; }
        public Action<string> SetText { get; set; }
        #endregion


        public TextBlock(XmlAttributeCollection attributes, IGuiContainer parent)
            :base(attributes, parent)
        {
            Text = attributes["Text"].ParseString();
            textAnchor = attributes["TextAnchor"].ParseVector2();

            SetText = setText;
        }

        protected virtual void setText(string text)
        {
            Text = text;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Point textPosition = (ContentBounds.Size.ToVector2() * textAnchor).ToPoint() - (font.MeasureString(Text) / 2).ToPoint() + ContentBounds.Location;

            spriteBatch.DrawString(font, Text, textPosition.ToVector2(), TextColour);
        }

        public override void RecalculateStyle(Style style)
        {
            base.RecalculateStyle(style);

            ElementStyle elementStyle = style.GetElementStyle(this, styleName);

            font = style.GetElementFont(elementStyle);

            TextColour = style.GetElementFontColour(elementStyle);
        }
    }
}
