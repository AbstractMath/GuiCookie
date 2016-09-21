using GuiCookie;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace TestingApp
{
    class TestModel : GuiContainer
    {

        TextBlock Timer;

        private bool hidden;
        public bool Hidden
        {
            get { return hidden; }
            set
            {
                hidden = value;
                string b = (value) ? "hidden" : "not hidden";
                Timer.Text = "Button1 is " + b;
            }
        }
        public TestModel(string GuiSheet, string StyleSheet, GameWindow Window, ContentManager Content, GraphicsDevice GraphicsDevice)
            : base(GuiSheet, StyleSheet, Window, Content, GraphicsDevice)
        {
            Timer = GetNamedElement("Timer") as TextBlock;
        }

        public override void ButtonPressed(string message)
        {
            switch (message)
            {
                case "HideButtonOne":
                    bool v = GetNamedElement("Button1").Visible;
                    GetNamedElement("Button1").Visible = !v;
                    Hidden = v;
                    break;
            }
        }

        
    }
}
