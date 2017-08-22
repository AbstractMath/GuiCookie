using GuiCookie.Elements;
using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GuiCookie
{
    public abstract partial class Root : IGuiContainer
    {
        #region PRIVATE PROPERTIES
        /// <summary> This GUI's style. </summary>
        private Style style;
        #endregion

        #region PUBLIC PROPERTIES
        /// <summary> The list of the root-level elements. </summary>
        public List<Element> Elements { get; private set; }

        /// <summary> The size of the overall GUI. </summary>
        public Rectangle ContentBounds { get => new Rectangle(0, 0, 800, 600); }
        #endregion

        #region CONSTRUCTORS
        /// <summary> Create a GUI model with the given GuiSheet, StyleSheet, and ContentManager. </summary>
        /// <param name="guiSheet"> The XML file to read the element data from. </param>
        /// <param name="styleSheet"> The XML file to read the style data from. </param>
        /// <param name="content"> The content manager to load the resources with. </param>
        public Root(string guiSheet, string styleSheet, ContentManager content)
        {
            //Loads the stylesheet
            style = new Style(styleSheet, content);

            //Loads the guisheet
            loadGuiSheet(guiSheet);

            //Calculates the textures of every element based on the stylesheet
            RecalculateElements();
        }
        #endregion

        #region IGUICONTAINER FUNCTIONS
        /// <summary> Gets the first element found with the matching ID. </summary>
        /// <param name="id"> The ID of the element. </param>
        /// <returns> The first element with this ID, or null if none is found. </returns>
        public Element GetElementByID(string id)
        {
            //If the id was not given, return null
            if (id == string.Empty || id == null)
                return null;

            //Go through each element in this container's list
            foreach (Element e in Elements)
            {
                //If the element's ID matches the given id, return it
                if (e.ID == id)
                    return e;

                //If the element is a container, tell it to check its children as well
                if (e is IGuiContainer g)
                {
                    Element eg = g.GetElementByID(id);
                    if (eg != null) return eg;
                }
            }

            //If no element is found, return null
            return null;
        }

        /// <summary> Add a single element to this container's elements. </summary>
        /// <param name="element"> The element to add. </param>
        public void AddElement(Element element) => Elements.Add(element);

        /// <summary> Add a list of elements to this container's elements. </summary>
        /// <param name="elements"> The list of elements to add. </param>
        public void AddElements(List<Element> elements) => Elements.AddRange(elements);
        #endregion

        #region ROOT FUNCTIONS
        /// <summary> Draws every element. </summary>
        /// <param name="spriteBatch"> The spriteBatch to draw with. </param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Element e in Elements)
                e.Draw(spriteBatch);
        }

        /// <summary> Updates every element. </summary>
        public virtual void Update()
        {
            foreach (Element e in Elements)
                e.Update();
        }

        /// <summary> Redraws every element. </summary>
        public void RecalculateElements()
        {
            foreach (Element e in Elements)
                e.RecalculateStyle(style);
        }
        #endregion
    }
}
