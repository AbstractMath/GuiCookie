using GuiCookie.Elements;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Xml;

namespace GuiCookie.StyleStructures
{
    public partial class Style
    {
        #region LOAD FUNCTIONS
        /// <summary> Loads the fonts and textures from an XmlNode representing the resources node in a stylesheet </summary>
        /// <param name="resourceNode"> The resources node </param>
        /// <param name="content"> The content manager to load the textures with </param>
        private void loadResources(XmlNode resourceNode, ContentManager content)
        {
            //Checks that the resource node exists and has children
            if (resourceNode == null) throw new Exception("Missing Resource node.");
            if (!resourceNode.HasChildNodes) throw new Exception("Resource node has no child nodes.");

            //Checks that the resource node has a root folder set
            string folderPath;
            if (resourceNode.Attributes.GetNamedItem("Folder") == null) throw new Exception("Resource node has no Folder attribute.");
            else folderPath = resourceNode.Attributes.GetNamedItem("Folder").Value;

            //Checks that the resource node has an image node
            XmlNode imageNode = resourceNode.SelectSingleNode("Images");
            if (imageNode == null) throw new Exception("Resource node is missing Images node.");

            //Sets the textures based on the nodes within the image node
            textures = new Dictionary<string, Texture2D>(imageNode.ChildNodes.Count);
            foreach (XmlNode image in imageNode)
            {
                string imageName, imageURI;

                //Checks that the image node has a name set
                if (image.Attributes.GetNamedItem("Name") == null) throw new Exception("Image node is missing Name attribute.");
                else imageName = image.Attributes.GetNamedItem("Name").Value;

                //Checks that the image node has a URI set
                if (image.Attributes.GetNamedItem("URI") == null) throw new Exception("Image node is missing URI attribute.");
                else imageURI = image.Attributes.GetNamedItem("URI").Value;

                //Loads the image and adds it to the dictionary keyed by its name
                textures[imageName] = content.Load<Texture2D>(folderPath + "/" + imageURI);
            }

            //Checks that the resource node has a font node
            XmlNode fontNode = resourceNode.SelectSingleNode("Fonts");
            if (fontNode == null) throw new Exception("Resource node is missing Fonts node.");

            //Sets the fonts based on the nodes within the font node
            fonts = new Dictionary<string, SpriteFont>(fontNode.ChildNodes.Count);
            foreach (XmlNode font in fontNode)
            {
                //The name and URI of this font
                string fontName, fontURI;

                //Checks that the font node has a name set
                if (font.Attributes.GetNamedItem("Name") == null) throw new Exception("Font node is missing Name attribute.");
                else fontName = font.Attributes.GetNamedItem("Name").Value;

                //Checks that the font node has a URI set
                if (font.Attributes.GetNamedItem("URI") == null) throw new Exception("Font node is missing URI attribute.");
                else fontURI = font.Attributes.GetNamedItem("URI").Value;

                //Loads the font and adds it to the dictionary keyed by its name
                fonts[fontName] = content.Load<SpriteFont>(folderPath + "/" + fontURI);
            }
        }

        /// <summary> Goes through the main style node and loads it into a list of data. </summary>
        /// <param name="styleNode"> The style node. </param>
        private void loadStyles(XmlNode styleNode)
        {
            //Checks that the style node exists and has children
            if (styleNode == null) throw new Exception("Missing Style node.");
            if (!styleNode.HasChildNodes) throw new Exception("Style node has no child nodes.");

            //Initialises the style list
            styles = new List<ElementStyle>(styleNode.ChildNodes.Count);

            //Goes through each element style node and adds it to the styles
            for (int s = 0; s < styleNode.ChildNodes.Count; s++)
            {
                //This style's node
                XmlNode elementStyleNode = styleNode.ChildNodes[s];

                //Creates a new ElementStyle to store this style
                ElementStyle style = new ElementStyle();

                //Sets the style's type, as long as the type is within the Element namespace
                if (typeof(Element).Assembly.GetType(typeof(Element).Namespace + "." + elementStyleNode.Name) != null) style.Type = typeof(Element).Assembly.GetType(typeof(Element).Namespace + "." + elementStyleNode.Name);
                else throw new Exception("Invalid style type, style node's name must match element name.");

                //Sets the style's name if supplied, otherwise defaults to string.Empty
                style.Name = (elementStyleNode.Attributes.GetNamedItem("Name") == null) ? string.Empty : elementStyleNode.Attributes.GetNamedItem("Name").Value;

                //Initialises the style's parameters array
                style.Parameters = new Dictionary<string, StyleParameter>(elementStyleNode.ChildNodes.Count);

                //Goes through each node within the element style node
                foreach (XmlNode styleParameter in elementStyleNode.ChildNodes)
                {
                    //Creates a StyleParameter with the given name
                    StyleParameter parameter = new StyleParameter()
                    {
                        Name = styleParameter.Name,
                        Parameters = new Dictionary<string, string>(styleParameter.Attributes.Count)
                    };

                    //Goes through each attribute of the parameter and adds it to the styleParameter's dictionary
                    foreach (XmlAttribute attribute in styleParameter.Attributes) parameter.Parameters[attribute.Name] = attribute.Value;

                    //Adds the parameter to the style
                    style.Parameters[styleParameter.Name] = parameter;
                }

                //Adds the style to the list
                styles.Add(style);
            }
        }
        #endregion
    }
}
