using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml;

namespace GuiCookie.Elements
{
    public class Button : TextBlock
    {
        #region PRIVATE PROPERTIES
        protected Texture2D hoverTexture;
        protected Texture2D defaultTexture;
        protected string hoverStyleName;
        protected Delegate onClick;
        protected object[] buttonParameters;
        #endregion



        public Button(XmlAttributeCollection attributes, IGuiContainer parent, Delegate clicked, object[] buttonParams)
            : base(attributes, parent)
        {
            hoverStyleName = attributes["Hover"].ParseString();

            if (clicked != null)
            {
                onClick = clicked;
                buttonParameters = buttonParams;
            }
        }

        private void clicked()
        {
            if (onClick != null)
                onClick.DynamicInvoke(buttonParameters);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //spriteBatch.Draw(hoverTexture, FullBounds, Color.White);

        }

        public override void Update()
        {
            base.Update();

            //Switch to the hovered over texture if the button is hovered over
            texture = (FullBounds.Contains(Mouse.GetState().Position)) ? hoverTexture : defaultTexture;

            if (FullBounds.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                clicked();
        }

        public override void RecalculateStyle(Style style)
        {
            base.RecalculateStyle(style);

            //Get the styles for default and hovered
            ElementStyle hoverStyle = style.GetElementStyle(this, (hoverStyleName == string.Empty) ? styleName : hoverStyleName);
            ElementStyle defaultStyle = style.GetElementStyle(this, styleName);

            //Create the textures
            defaultTexture = style.CreateElementTexture(defaultStyle, FullBounds);
            hoverTexture = style.CreateElementTexture(hoverStyle, FullBounds);
        }
    }
}
