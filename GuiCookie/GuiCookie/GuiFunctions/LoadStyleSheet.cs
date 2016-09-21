using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml;

namespace GuiCookie
{
    public static partial class GuiFunctions
    {
        public static StyleSettings LoadStyleSheet(string FileName, ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            //Attempts to load the document
            XmlDocument sheetDocument = new XmlDocument();
            try
            {
                sheetDocument.Load(FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error '{0}' occured", e);
                return new StyleSettings();
            }
            //The graphics object
            StyleSettings styleSettings = new StyleSettings();
            styleSettings.Fonts = new Dictionary<string, SpriteFont>();
            styleSettings.ButtonStyles = new Dictionary<string, BoxStyle>();
            //The main node
            XmlNode mainNode = sheetDocument.GetElementsByTagName("Style")[0];


            //The folder that the style items are in
            string styleFolder = mainNode.Attributes.GetNamedItem("Folder").Value;
            //Loads the fonts
            foreach (XmlNode f in mainNode.FirstChild.ChildNodes)
            {
                //The name of the new font, used to key the item
                string name = f.Attributes.GetNamedItem("Name").Value;
                //The URI of the new font, appended to the folder name to create a path
                string URI = f.Attributes.GetNamedItem("URI").Value;
                //Loads the font found at the styleFolder + the URI of the item and stores it in the dictionary keyed with its name
                SpriteFont font = Content.Load<SpriteFont>(styleFolder + "/" + URI);
                styleSettings.Fonts.Add(name, font);
            }
            //Goes through all the nodes in the main node
            foreach (XmlNode n in mainNode.ChildNodes)
            {
                switch (n.Name)
                {
                    case "BoxStyle":
                        //Makes a new ButtonStyle
                        BoxStyle b = new BoxStyle();
                        //Set the font of the ButtonStyle
                        b.Font = styleSettings.Fonts[n.Attributes.GetNamedItem("Font").Value];
                        //Loads the main image containing the parts of the button
                        Texture2D ButtonStyleImage = Content.Load<Texture2D>(styleFolder + "/" + n.Attributes.GetNamedItem("URI").Value);
                        //The nodes
                        XmlNode cornerNode = n.SelectNodes("CornerTexture")[0].Attributes.GetNamedItem("Source");
                        XmlNode edgeNode = n.SelectNodes("EdgeTexture")[0].Attributes.GetNamedItem("Source");
                        XmlNode fillNode = n.SelectNodes("FillTexture")[0].Attributes.GetNamedItem("Source");
                        b.CornerTexture = GetTexture(ButtonStyleImage, GetRectangleFromNode(cornerNode));
                        b.EdgeTexture = GetTexture(ButtonStyleImage, GetRectangleFromNode(edgeNode));
                        b.FillTexture = GetTexture(ButtonStyleImage, GetRectangleFromNode(fillNode));
                        b.Tiled = Convert.ToBoolean(n.SelectNodes("FillTexture")[0].Attributes.GetNamedItem("Tiled").Value);
                        styleSettings.ButtonStyles.Add(n.Attributes.GetNamedItem("Name").Value, b);
                        break;
                }
            }
            return styleSettings;
        }



        private static Texture2D GetTexture(Texture2D Image, Rectangle Source)
        {
            Color[] newData = new Color[Source.Width * Source.Height];
            Image.GetData(0, Source, newData, 0, newData.Length);
            Texture2D newTexture = new Texture2D(Image.GraphicsDevice, Source.Width, Source.Height);
            newTexture.SetData(newData);
            return newTexture;
        }

        public static Texture2D ConstructElementTexture(BoxStyle Style, Vector2 Size)
        {
            Vector2 cornerSize = Style.CornerTexture.Bounds.Size.ToVector2();
            Vector2 edgeSize = Style.EdgeTexture.Bounds.Size.ToVector2();
            Vector2 fillSize = Style.FillTexture.Bounds.Size.ToVector2();
            GraphicsDevice graphicsDevice = Style.CornerTexture.GraphicsDevice;
            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, (int)Size.X, (int)Size.Y);

            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1.0f, 0);
            using (SpriteBatch sprite = new SpriteBatch(graphicsDevice))
            {
                sprite.Begin();
                //Draws the corners
                sprite.Draw(Style.CornerTexture, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                sprite.Draw(Style.CornerTexture, new Vector2(Size.X - cornerSize.X, 0), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 1);
                sprite.Draw(Style.CornerTexture, new Vector2(0, Size.Y - cornerSize.Y), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.FlipVertically, 1);
                sprite.Draw(Style.CornerTexture, new Vector2(Size.X - cornerSize.X, Size.Y - cornerSize.Y), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically, 1);
                //Draws the edges
                for (int i = (int)cornerSize.X; i < Size.X - cornerSize.X; i += (int)edgeSize.X)
                {
                    //If it'll overlap the corner, trim it down
                    Texture2D edgeTexture = ((Size.X - cornerSize.X) - i < edgeSize.X) ? GetTexture(Style.EdgeTexture, new Rectangle(0, 0, (int)(Size.X - cornerSize.X) - i, (int)edgeSize.Y)) : Style.EdgeTexture;
                    sprite.Draw(edgeTexture, new Vector2(i, 0), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 1);
                    sprite.Draw(edgeTexture, new Vector2(i, Size.Y - edgeSize.Y), null, Color.White, 0, new Vector2(0, 0), 1f, SpriteEffects.FlipVertically, 1);
                }
                for (int i = (int)cornerSize.Y; i < Size.Y - cornerSize.Y; i += (int)edgeSize.X)
                {
                    Texture2D edgeTexture = ((Size.Y - cornerSize.Y) - i < edgeSize.X) ? GetTexture(Style.EdgeTexture, new Rectangle(0, 0, (int)(Size.Y - cornerSize.Y) - i, (int)edgeSize.Y)) : Style.EdgeTexture;
                    sprite.Draw(edgeTexture, new Vector2(0, i), null, Color.White, 1.5708f, new Vector2(0, edgeSize.Y), 1f, SpriteEffects.FlipVertically, 1);
                    sprite.Draw(edgeTexture, new Vector2(Size.X - edgeSize.Y, i), null, Color.White, 1.5708f, new Vector2(0, edgeSize.Y), 1f, SpriteEffects.None, 1);
                }
                //Draws the fill
                Rectangle fillRect = new Rectangle((int)cornerSize.X, (int)cornerSize.Y, (int)(Size.X - cornerSize.X * 2), (int)(Size.Y - cornerSize.Y * 2));
                if (Style.Tiled)
                {
                    for (int x = fillRect.Left; x < fillRect.Right; x += (int)fillSize.X)
                    {
                        for (int y = fillRect.Top; y < fillRect.Bottom; y += (int)fillSize.Y)
                        {
                            int fillWidth = (x + fillSize.X > fillRect.Right) ? fillRect.Right - x : (int)fillSize.X;
                            int fillHeight = (y + fillSize.Y > fillRect.Bottom) ? fillRect.Bottom - y : (int)fillSize.Y;
                            Texture2D fillTexture = (fillWidth != fillSize.X || fillHeight != fillSize.Y) ?
                                GetTexture(Style.FillTexture, new Rectangle(0, 0, fillWidth, fillHeight)) : Style.FillTexture;
                            sprite.Draw(fillTexture, new Vector2(x, y), Color.White);
                        }
                    }
                }
                else
                {
                    sprite.Draw(Style.FillTexture, fillRect, Color.White);
                }

                sprite.End();
            }

            Texture2D finalTexture = renderTarget;
            graphicsDevice.SetRenderTarget(null);
            return finalTexture;
        }

    }
}
