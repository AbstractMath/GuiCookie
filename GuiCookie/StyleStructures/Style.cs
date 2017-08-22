using GuiCookie.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GuiCookie.StyleStructures
{
    public partial class Style
    {
        #region PRIVATE PROPERTIES
        /// <summary> The list of loaded styles. </summary>
        private List<ElementStyle> styles;

        /// <summary> The dictionary of textures by style name. </summary>
        private Dictionary<string, Texture2D> textures;

        /// <summary> The dictionary of fonts by style name. </summary>
        private Dictionary<string, SpriteFont> fonts;
        #endregion

        #region CONSTRUCTORS
        /// <summary> Create and load a new style from a file. </summary>
        /// <param name="filePath"> The full path of the stylesheet. </param>
        /// <param name="content"> The ContentManager to load the style with. </param>
        public Style(string filePath, ContentManager content)
        {
            XmlDocument styleSheet = new XmlDocument();

            styleSheet.Load(filePath);

            XmlNode rootNode = styleSheet.SelectSingleNode("Body");

            loadResources(rootNode.SelectSingleNode("Resources"), content);

            loadStyles(rootNode.SelectSingleNode("Style"));
        }
        #endregion

        /// <summary> Gets the best suited style from the given element. </summary>
        /// <param name="element"> The element to find a style for. </param>
        /// <returns> The best suited style. </returns>
        public ElementStyle GetElementStyle(Element element, string styleName)
        {
            //Creates a new blank style to return
            ElementStyle finalStyle = new ElementStyle() { Parameters = new Dictionary<string, StyleParameter>() };

            //Finds all styles in the main list that are valid for the given element, then sort them from least to most relevant
            List<ElementStyle> applicableStyles = styles.FindAll(s => element.GetType().DerivesFrom(s.Type) && (s.Name == styleName || s.Name == string.Empty));
            applicableStyles = applicableStyles.OrderBy(s => s.Type.InheritanceLevel(element.GetType())).ThenBy(s => s.Name).ToList();

            //Go through each applicable style
            foreach (ElementStyle style in applicableStyles)
                foreach (StyleParameter parameter in style.Parameters.Values)
                {
                    //If the parameters doesn't exist, make a new StyleParameter for it. This avoids reference type fuckery
                    if (!finalStyle.Parameters.ContainsKey(parameter.Name))
                        finalStyle.Parameters[parameter.Name] = new StyleParameter() { Parameters = new Dictionary<string, string>() };

                    //Copy all the parameters, overwriting any old ones
                    foreach (KeyValuePair<string, string> subParam in parameter.Parameters)
                        finalStyle.Parameters[parameter.Name].Parameters[subParam.Key] = subParam.Value;

                }

            //Once the finalStyle has been overwritten fully, return it
            return finalStyle;
        }

        /// <summary> Creates a texture for the given element. </summary>
        /// <param name="elementStyle"> The style of the element. </param>
        /// <param name="bounds"> The bounds of the element. </param>
        /// <returns> The element texture. </returns>
        public Texture2D CreateElementTexture(ElementStyle elementStyle, Rectangle bounds)
        {
            //The graphics device to load the textures onto
            GraphicsDevice graphicsDevice = textures.First().Value.GraphicsDevice;

            //A 3x3 array of the corners, edges, and fill of the texture. Imagine the texture being split into 3s based on the edges and corners
            //Calculates the borders and fills in the edges of the array with the relevant textures
            Texture2D[,] textureComponents = calculateBorder(elementStyle, graphicsDevice);

            //Sets the centre of the texture to the background texture
            textureComponents[1, 1] = calculateBackground(elementStyle, graphicsDevice);

            //Create the full texture from the components and return it
            return constructElementTexture(elementStyle, graphicsDevice, textureComponents, bounds);
        }

        public SpriteFont GetElementFont(ElementStyle elementStyle)
        {
            //Throw an error if the text node doesn't exist
            if (!elementStyle.Parameters.ContainsKey("Text"))
                throw new Exception("Text node was missing from style node.");

            //Throw an error if the font parameter doesn't exist
            if (!elementStyle.Parameters["Text"].Parameters.ContainsKey("Font"))
                throw new Exception("Font parameter was missing from Text node.");

            //Throw an error if the font doesn't exist
            if (!fonts.ContainsKey(elementStyle.Parameters["Text"].Parameters["Font"]))
                throw new Exception("Font parameter does not match SpriteFont defined in Resources node.");

            return fonts[elementStyle.Parameters["Text"].Parameters["Font"]];
        }

        public Color GetElementFontColour(ElementStyle elementStyle)
        {
            //Throw an error if the text node doesn't exist
            if (!elementStyle.Parameters.ContainsKey("Text"))
                throw new Exception("Text node was missing from style node.");

            if (elementStyle.Parameters["Text"].Parameters.ContainsKey("Colour"))
                return elementStyle.Parameters["Text"].Parameters["Colour"].ParseColour();
            else
                return Color.Black;
        }
    }
}
