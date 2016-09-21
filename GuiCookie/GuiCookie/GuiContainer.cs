using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GuiCookie
{
    public abstract class GuiContainer : IGuiContainer
    {
        private List<Element> Elements;
        private Vector2 DesignResolution;
        private StyleSettings StyleSettings;
        private GraphicsDevice GraphicsDevice;
        private SpriteBatch SpriteBatch;
        private Vector2 Scale;
        public Vector2 Position { get { return Vector2.Zero; } }

        //public Matrix Transform
        //{
        //    get
        //    {
        //        Matrix matrix;
        //        matrix = Matrix.CreateScale(new Vector3(Scale.X, Scale.Y, 0)) *
        //            Matrix.CreateRotationZ(0) *
        //            Matrix.CreateTranslation(Vector3.Zero);
        //        return matrix;
        //    }
        //}

        public GuiContainer(string GuiSheet, string StyleSheet, GameWindow Window, ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            this.GraphicsDevice = GraphicsDevice;
            Elements = new List<Element>(5);
           
            StyleSettings = GuiFunctions.LoadStyleSheet(StyleSheet, Content, GraphicsDevice);
            GuiFunctions.LoadGuiSheet(GuiSheet, StyleSettings, ButtonPressed, this);
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Window.ClientSizeChanged += ScaleChanged;
        }

        public void SetData(GuiData Data)
        {
            Elements = Data.Elements;
            DesignResolution = Data.DesignResolution;
        }

        private void ScaleChanged(object sender, EventArgs e)
        {
            GameWindow window = sender as GameWindow;
            Scale = new Vector2(window.ClientBounds.Width / DesignResolution.X, window.ClientBounds.Height / DesignResolution.Y);
        }

        public virtual void ButtonPressed(string message)
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

        public void AddElement(Element Element)
        {
            Elements.Add(Element);
        }

        //This will draw each element
        public void Draw()
        {
            //SpriteBatch.Begin(SpriteSortMode.Deferred,
            //    BlendState.AlphaBlend,
            //    SamplerState.PointClamp,
            //    null, null, null,
            //    Transform);
            SpriteBatch.Begin();
            foreach (Element e in Elements)
            {
                e.Draw(SpriteBatch);
            }
            SpriteBatch.End();
        }

        //This will update each element
        public void Update(GameTime gameTime)
        {
            foreach (Element e in Elements)
            {
                e.Update(gameTime);
            }
        }
    }
    public struct GuiData
    {
        public List<Element> Elements;
        public Vector2 DesignResolution;
    }
}
