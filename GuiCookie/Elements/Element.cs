using GuiCookie.DataTypes;
using GuiCookie.StyleStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;

namespace GuiCookie.Elements
{
    public abstract class Element
    {
        #region PRIVATE PROPERTIES
        protected Texture2D texture;
        protected Bounding bounding;
        protected string styleName;
        #endregion

        #region PUBLIC PROPERTIES
        public IGuiContainer Parent { get; protected set; }
        
        public string Name { get; set; }
        public string ID { get; set; }
        
        public Rectangle ContentBounds { get => bounding.AbsoluteContentBounds; }
        public Rectangle FullBounds { get => bounding.AbsoluteBounds; }
        #endregion

        internal Element(XmlAttributeCollection attributes, IGuiContainer parent)
        {
            //Set the StyleName, Name, and ID
            styleName = attributes["Style"].ParseString();
            Name = attributes["Name"].ParseString();
            ID = attributes["ID"].ParseString();

            Parent = parent;

            bounding = new Bounding(attributes["Anchor"], attributes["Position"], attributes["Size"], attributes["Padding"], parent.ContentBounds);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounding.AbsoluteBounds, Color.White);
        }

        public virtual void Update()
        {

        }

        public virtual void RecalculateStyle(Style style)
        {
            ElementStyle elementStyle = style.GetElementStyle(this, styleName);

            texture = style.CreateElementTexture(elementStyle, FullBounds);
        }

    }
}
