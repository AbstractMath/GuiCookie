using Microsoft.Xna.Framework;
using System.Xml;

namespace GuiCookie.DataTypes
{
    public class Bounding
    {
        #region PUBLIC PROPERTIES
        /// <summary> The anchor of the bounds. </summary>
        public Vector2 Anchor { get; set; }

        /// <summary> The position. </summary>
        public GuiPoint Position { get; set; }

        /// <summary> The size. </summary>
        public GuiPoint Size { get; set; }

        /// <summary> The padding. </summary>
        public Padding Padding { get; set; }

        /// <summary> The bounds which contains this bounding, used to calculate relative positioning and sizing. </summary>
        public Rectangle ParentContentBounds { get; set; }
        #endregion

        #region INTERNAL CONSTRUCTORS
        /// <summary> Create a new Bounding with the given attributes and a parent container. </summary>
        /// <param name="anchorAttribute"> The attribute for the anchor. </param>
        /// <param name="positionAttribute"> The attribute for the position. </param>
        /// <param name="sizeAttribute"> The attribute for the size. </param>
        /// <param name="paddingAttribute"> The attribute for the padding. </param>
        /// <param name="container"> The parent container. </param>
        internal Bounding(XmlAttribute anchorAttribute, XmlAttribute positionAttribute, XmlAttribute sizeAttribute, XmlAttribute paddingAttribute, Rectangle container)
        {
            Anchor = anchorAttribute.ParseVector2();
            Position = new GuiPoint(positionAttribute);
            Size = new GuiPoint(sizeAttribute);
            Padding = new Padding(paddingAttribute);
            ParentContentBounds = container;
        }
        #endregion

        #region CALCULATED PROPERTIES
        /// <summary> The full bounds relative to itself. </summary>
        public Rectangle RelativeBounds
        {
            get
            {
                //Calculate the width and the height
                int width = (int)((Size.RelativeX) ? ParentContentBounds.Width * Size.X : Size.X);
                int height = (int)((Size.RelativeY) ? ParentContentBounds.Height * Size.Y : Size.Y);

                //Calculate the offset based on the anchor
                int offsetX = (int)((Size.RelativeX) ? width * Anchor.X : Size.X * Anchor.X);
                int offsetY = (int)((Size.RelativeY) ? height * Anchor.Y : Size.Y * Anchor.Y);

                //Calculate the position based on the parent's bounds
                int x = (int)((Position.RelativeX) ? ParentContentBounds.Width * Position.X : Position.X) - offsetX;
                int y = (int)((Position.RelativeY) ? ParentContentBounds.Height * Position.Y : Position.Y) - offsetY;

                //Return the final rectangle
                return new Rectangle(x, y, width, height);
            }
        }

        /// <summary> The full bounds in screen-space. </summary>
        public Rectangle AbsoluteBounds { get => new Rectangle(RelativeBounds.Location + ParentContentBounds.Location, RelativeBounds.Size); }

        /// <summary> The content bounds relative to itself. </summary>
        public Rectangle RelativeContentbounds
        {
            get
            {
                //the full bounds of this bounding
                Rectangle fullBounds = RelativeBounds;

                //Calculate the padding of each side
                int left = (int)(Padding.RelativeLeft ? fullBounds.Width * Padding.Left : Padding.Left);
                int top = (int)(Padding.RelativeTop ? fullBounds.Height * Padding.Top : Padding.Top);
                int right = (int)(Padding.RelativeRight ? fullBounds.Width * Padding.Right : Padding.Right);
                int bottom = (int)(Padding.RelativeBottom ? fullBounds.Height * Padding.Bottom : Padding.Bottom);

                //Calculate the size
                int width = fullBounds.Width - (left + right);
                int height = fullBounds.Height - (top + bottom);

                //Return the final rectangle
                return new Rectangle(left, top, width, height);
            }
        }

        /// <summary> The content bounds in screen-space. </summary>
        public Rectangle AbsoluteContentBounds { get => new Rectangle(RelativeContentbounds.Location + AbsoluteBounds.Location, RelativeContentbounds.Size); }
        #endregion
    }
}
