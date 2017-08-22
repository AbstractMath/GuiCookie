using GuiCookie.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace GuiCookie.StyleStructures
{
    // Handles creation of textures for elements.
    public partial class Style
    {
       #region DRAW FUNCTIONS
        /// <summary> Pieces together the components of the element texture and constructs it. </summary>
        /// <param name="elementStyle"> The style of the element. </param>
        /// <param name="graphicsDevice"> The graphics device to create the texture with. </param>
        /// <param name="components"> The components of the texture. </param>
        /// <param name="bounds"> The bounds of the element. </param>
        /// <returns> The constructed texture. </returns>
        private Texture2D constructElementTexture(ElementStyle elementStyle, GraphicsDevice graphicsDevice, Texture2D[,] components, Rectangle bounds)
        {
            //Tints the texture based on the given colour, or doesn't tint at all if no colour is given
            Color backgroundColour = (elementStyle.Parameters.ContainsKey("Background") && elementStyle.Parameters["Background"].Parameters.ContainsKey("Colour"))
                ? elementStyle.Parameters["Background"].Parameters["Colour"].ParseColour() : Color.White;
            Color borderColour = (elementStyle.Parameters.ContainsKey("Border") && elementStyle.Parameters["Border"].Parameters.ContainsKey("Colour"))
                ? elementStyle.Parameters["Border"].Parameters["Colour"].ParseColour() : Color.White;

            //If the background should tile
            bool tileBackground = (elementStyle.Parameters.ContainsKey("Background") && elementStyle.Parameters["Background"].Parameters.ContainsKey("Tiled"))
                ? bool.Parse(elementStyle.Parameters["Background"].Parameters["Tiled"]) : true;

            //Changes what the spritebatch draws to, so it can be saved to a Texture2D
            RenderTarget2D target = new RenderTarget2D(graphicsDevice, bounds.Width, bounds.Height);
            graphicsDevice.SetRenderTarget(target);
            graphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Transparent, 1f, 0);

            //Creates a new spritebatch to draw to the target
            using (SpriteBatch sprite = new SpriteBatch(graphicsDevice))
            {
                //Begins drawing
                sprite.Begin();

                //Draws the background
                drawBackground(sprite, components, target.Bounds, backgroundColour, tileBackground);

                //Draws the edges
                drawEdges(sprite, components, target.Bounds, borderColour);

                //Draws the corners
                drawCorners(sprite, components, target.Bounds, borderColour);

                //Finishes drawing
                sprite.End();
            }

            //Creates a texture from the target
            Texture2D elementTexture = target;

            //Sets the render target back to the screen
            graphicsDevice.SetRenderTarget(null);

            //Returns the final texture
            return elementTexture;
        }

        /// <summary> Draws the background onto the RenderTarget. </summary>
        /// <param name="sprite"> The spritebatch to draw with. </param>
        /// <param name="components"> The texture components of the element. </param>
        /// <param name="bounds"> The bounds of the element. </param>
        /// <param name="backgroundColour"> The colour to tint the texture. </param>
        /// <param name="tiled"> Whether or not the background should tile. </param>
        private void drawBackground(SpriteBatch sprite, Texture2D[,] components, Rectangle bounds, Color backgroundColour, bool tiled)
        {
            //Calculates how big the background has to be to ensure nothing is left empty
            int startX = Math.Min(components[0, 0].Width, components[0, 1].Width), startY = Math.Min(components[0, 0].Height, components[1, 0].Height),
                endX = bounds.Width - Math.Max(components[2, 2].Width, components[2, 1].Width), endY = bounds.Height - Math.Max(components[2, 2].Height, components[1, 2].Height);

            //If the background is tiled, tile the texture
            if (tiled)
                for (int x = startX; x < endX; x += components[1, 1].Width)
                    for (int y = startY; y < endY; y += components[1, 1].Height)
                        sprite.Draw(components[1, 1], new Vector2(x, y), new Rectangle(0, 0, Math.Min(components[1, 1].Width, endX - x), Math.Min(components[1, 1].Height, endY - y)), backgroundColour);

            //Otherwise, just stretch the texture out
            else
                sprite.Draw(components[1, 1], new Rectangle(startX, startY, endX - startX, endY - startY), backgroundColour);
        }

        /// <summary> Draws the corners of an element onto the RenderTarget. </summary>
        /// <param name="sprite"> The spritebatch to draw with. </param>
        /// <param name="components"> The texture components of the element. </param>
        /// <param name="bounds"> The bounds of the element. </param>
        /// <param name="borderColour"> The colour to tint the texture. </param>
        private void drawCorners(SpriteBatch sprite, Texture2D[,] components, Rectangle bounds, Color borderColour)
        {
            //Draw the top-left corner
            sprite.Draw(components[0, 0], Vector2.Zero, borderColour);
            
            //Draw the top-right corner
            sprite.Draw(components[2, 0], new Vector2(bounds.Width - components[2, 0].Width, 0), borderColour);

            //Draw the bottom-left corner
            sprite.Draw(components[0, 2], new Vector2(0, bounds.Height - components[0, 2].Height), borderColour);

            //Draw the bottom-right corner
            sprite.Draw(components[2, 2], new Vector2(bounds.Width - components[2, 2].Width, bounds.Height - components[2, 2].Height), borderColour);
        }

        /// <summary> Draws the edges of an element onto the RenderTarget. </summary>
        /// <param name="sprite"> The spritebatch to draw with. </param>
        /// <param name="components"> The texture components of the element. </param>
        /// <param name="bounds"> The bounds of the element. </param>
        /// <param name="borderColour"> The colour to tint the texture. </param>
        private void drawEdges(SpriteBatch sprite, Texture2D[,] components, Rectangle bounds, Color borderColour)
        {
            //Start from the top-left corner, go downwards to the bottom-left corner
            for (int y = components[0, 0].Height; y < bounds.Height - components[0, 2].Height; y += components[0, 1].Height)
                sprite.Draw(components[0, 1], new Vector2(0, y), new Rectangle(0, 0, components[0, 1].Width, Math.Min(components[0, 1].Height, (bounds.Height - components[0, 2].Height) - y)), borderColour);

            //Start from the top-right corner, go downwards to the bottom-right corner
            for (int y = components[2, 0].Height; y < bounds.Height - components[2, 2].Height; y += components[2, 1].Height)
                sprite.Draw(components[2, 1], new Vector2(bounds.Width - components[2, 1].Width, y),
                    new Rectangle(0, 0, components[2, 1].Width, Math.Min(components[2, 1].Height, (bounds.Height - components[2, 2].Height) - y)), borderColour);

            //Start from the top-left corner, go rightwards to the top-right corner
            for (int x = components[0, 0].Width; x < bounds.Width - components[2, 0].Width; x += components[1, 0].Width)
                sprite.Draw(components[1, 0], new Vector2(x, 0), new Rectangle(0, 0, Math.Min(components[1, 0].Width, (bounds.Width - components[2, 0].Width) - x), components[1, 0].Height), borderColour);

            //Start from the bottom-left corner, go rightwards to the bottom-right corner
            for (int x = components[0, 2].Width; x < bounds.Width - components[2, 2].Width; x += components[1, 2].Width)
                sprite.Draw(components[1, 2], new Vector2(x, bounds.Height - components[1, 2].Height),
                    new Rectangle(0, 0, Math.Min(components[1, 2].Width, (bounds.Width - components[2, 2].Width) - x), components[1, 2].Height), borderColour);
        }
        #endregion

        #region CALCULATE FUNCTIONS
        /// <summary> Calculates the corners and edges of the border and fills them into a 3x3 array. </summary>
        /// <param name="elementStyle"> The style of the element. </param>
        /// <param name="graphicsDevice"> The graphics device to load onto. </param>
        /// <returns> A 3x3 Texture2D array with the edges and corners filled in. </returns>
        private Texture2D[,] calculateBorder(ElementStyle elementStyle, GraphicsDevice graphicsDevice)
        {
            //A new texture to be returned
            Texture2D borderAtlas;

            //Information about the style parameters
            bool containsBorderNode = elementStyle.Parameters.ContainsKey("Border");
            bool containsImageParameter = containsBorderNode ? elementStyle.Parameters["Border"].Parameters.ContainsKey("Image") : false;
            bool containsColourParameter = containsBorderNode ? elementStyle.Parameters["Border"].Parameters.ContainsKey("Colour") : false;

            //If there's no border style node or no image name parameter, return a default background
            if (!containsBorderNode || (containsBorderNode && !containsImageParameter))
            {
                //Create a 1x1 texture
                borderAtlas = new Texture2D(graphicsDevice, 1, 1);

                //Since we know there's no image parameter, check if there's a colour parameter and use that to colour the texture instead of the default colour
                Color borderColour = containsColourParameter ? elementStyle.Parameters["Border"].Parameters["Colour"].ParseColour() : Color.Black;

                //Set the texture to the colour
                borderAtlas.SetData(new Color[1] { borderColour });

                //Use this texture for the edges of an array and return it
                return new Texture2D[3, 3]
                {
                    { borderAtlas, borderAtlas, borderAtlas },
                    { borderAtlas, null,        borderAtlas },
                    { borderAtlas, borderAtlas, borderAtlas }
                };
            }

            //The border node of the style
            StyleParameter borderParameter = elementStyle.Parameters["Border"];

            //If the texture doesn't exist within the list, throw an error
            if (!textures.ContainsKey(borderParameter.Parameters["Image"]))
                throw new Exception("Image parameter does not match texture defined in Resources node.");

            //The atlas that the edges and corners are cut from
            borderAtlas = textures[borderParameter.Parameters["Image"]];

            //Initialises the array
            Texture2D[,] borderArray = new Texture2D[3, 3];

            //Local function that cuts from the given atlas using the given string rectangle
            Texture2D calculateBorderPiece(string source, Texture2D atlas) => atlas.GetTexture(source.ParseRectangle());

            //Given a corner source, all corners will use this unless specified otherwise. Defaults to the atlas
            Texture2D defaultCorner = (borderParameter.Parameters.ContainsKey("Corner")) ? calculateBorderPiece(borderParameter.Parameters["Corner"], borderAtlas) : borderAtlas;

            //Sets the corners
            borderArray[0, 0] = (borderParameter.Parameters.ContainsKey("TopLeft-Corner")) ? calculateBorderPiece(borderParameter.Parameters["TopLeft-Corner"], borderAtlas) : defaultCorner;
            borderArray[2, 0] = (borderParameter.Parameters.ContainsKey("TopRight-Corner")) ? calculateBorderPiece(borderParameter.Parameters["TopRight-Corner"], borderAtlas) : defaultCorner.Rotate(1);
            borderArray[0, 2] = (borderParameter.Parameters.ContainsKey("BottomLeft-Corner")) ? calculateBorderPiece(borderParameter.Parameters["BottomLeft-Corner"], borderAtlas) : defaultCorner.Rotate(3);
            borderArray[2, 2] = (borderParameter.Parameters.ContainsKey("BottomRight-Corner")) ? calculateBorderPiece(borderParameter.Parameters["BottomRight-Corner"], borderAtlas) : defaultCorner.Rotate(2);

            //Given an edge source, all edges will use this unless specified otherwise. Defaults to the atlas
            Texture2D defaultEdge = (borderParameter.Parameters.ContainsKey("Edge")) ? calculateBorderPiece(borderParameter.Parameters["Edge"], borderAtlas) : borderAtlas;

            //Sets the edges
            borderArray[1, 0] = (borderParameter.Parameters.ContainsKey("Top-Edge")) ? calculateBorderPiece(borderParameter.Parameters["Top-Edge"], borderAtlas) : defaultEdge;
            borderArray[0, 1] = (borderParameter.Parameters.ContainsKey("Left-Edge")) ? calculateBorderPiece(borderParameter.Parameters["Left-Edge"], borderAtlas) : defaultEdge.Rotate(3);
            borderArray[1, 2] = (borderParameter.Parameters.ContainsKey("Bottom-Edge")) ? calculateBorderPiece(borderParameter.Parameters["Bottom-Edge"], borderAtlas) : defaultEdge.Rotate(2);
            borderArray[2, 1] = (borderParameter.Parameters.ContainsKey("Right-Edge")) ? calculateBorderPiece(borderParameter.Parameters["Right-Edge"], borderAtlas) : defaultEdge.Rotate(1);

            //Returns the final array
            return borderArray;
        }

        /// <summary> Cuts a texture from the main atlas based on given parameters. </summary>
        /// <param name="elementStyle"> The style of the element to read. </param>
        /// <param name="graphicsDevice"> The graphics device to create the texture on. </param>
        /// <returns></returns>
        private Texture2D calculateBackground(ElementStyle elementStyle, GraphicsDevice graphicsDevice)
        {
            //A new texture to be returned
            Texture2D backgroundTexture;

            //Information about the style parameters
            bool containsBackgroundNode = elementStyle.Parameters.ContainsKey("Background");
            bool containsImageParameter = containsBackgroundNode ? elementStyle.Parameters["Background"].Parameters.ContainsKey("Image") : false;
            bool containsColourParameter = containsBackgroundNode ? elementStyle.Parameters["Background"].Parameters.ContainsKey("Colour") : false;

            //If there's no background style node or no image name parameter, return a default background
            if (!containsBackgroundNode || (containsBackgroundNode && !containsImageParameter))
            {
                //Create a 1x1 texture
                backgroundTexture = new Texture2D(graphicsDevice, 1, 1);

                //Since we know there's no image parameter, check if there's a colour parameter and use that to colour the texture instead of the default colour
                Color backgroundColour = containsColourParameter ? elementStyle.Parameters["Background"].Parameters["Colour"].ParseColour() : Color.Honeydew;

                //Set the texture to the colour and return it
                backgroundTexture.SetData(new Color[1] { backgroundColour });
                return backgroundTexture;
            }

            //The Background node of the style
            StyleParameter backgroundParameter = elementStyle.Parameters["Background"];

            //If the texture doesn't exist within the list, throw an error
            if (!textures.ContainsKey(backgroundParameter.Parameters["Image"]))
                throw new Exception("Image parameter does not match texture defined in Resources node.");

            //The main image the background will be cut from
            Texture2D backgroundAtlas = textures[backgroundParameter.Parameters["Image"]];

            //If a source is given, return that cut from the atlas, otherwise return the atlas itself
            if (backgroundParameter.Parameters.ContainsKey("Source"))
                return backgroundAtlas.GetTexture(backgroundParameter.Parameters["Source"].ParseRectangle());
            else
                return backgroundAtlas;
        }
        #endregion
    }
}
