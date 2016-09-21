using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiCookie
{
    public class Button : Element, IClickable
    {
        private string Message;
        private MouseState lastMouse = Mouse.GetState();
        private string Text;
        private BoxStyle ButtonStyle;
        private Texture2D ButtonTexture;

        public Button(ButtonTemplate Template)
            :base(Template)
        {
            Message = Template.Message;
            Text = Template.Text;
            ButtonStyle = Template.ButtonStyle;
            ButtonTexture = GuiFunctions.ConstructElementTexture(ButtonStyle, Size);
        }
        
        public event OnClicked Clicked;

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible) return;
            spriteBatch.Draw(ButtonTexture, Position);
            spriteBatch.DrawString(ButtonStyle.Font, Text, new Vector2(Position.X + ((Size.X - ButtonStyle.Font.MeasureString(Text).X)) / 2, Position.Y + ((Size.Y - ButtonStyle.Font.MeasureString(Text).Y) / 2)), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Visible) return;
            //If the mouse is over the button and clicked
            if (BoundingRect.Contains(Mouse.GetState().Position) && Mouse.GetState().LeftButton == ButtonState.Released && lastMouse.LeftButton == ButtonState.Pressed)
            {
                Clicked.Invoke(Message);
            }
            lastMouse = Mouse.GetState();
        }   
    }

    public class ButtonTemplate : ElementTemplate
    {
        public string Text;
        public string Message;
        public BoxStyle ButtonStyle;
    }
}
