using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GuiCookie
{
    public abstract class Element
    {
        public Vector2 Position { get { return BoundingRect.Location.ToVector2(); } }
        public Vector2 Size { get { return BoundingRect.Size.ToVector2(); } }
        public Vector2 Offset { private set; get; }
        public Rectangle BoundingRect { get; set; }
        public string Name { get; set; }
        public bool Visible { get; set; }
        public IGuiContainer Parent { get; private set; }

        public Element(ElementTemplate Template)
        {
            Parent = Template.Parent;
            Offset = (Parent == null) ? Vector2.Zero : Parent.Position;
            BoundingRect = Template.Bounding;
            Visible = Template.Visible;
            Name = Template.Name;
            
        }

        //This will draw each element
        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        //This will update each element
        public virtual void Update(GameTime gameTime)
        {
            
        }
    }

    public class ElementTemplate
    {
        public Rectangle Bounding;
        public bool Visible;
        public string Name;
        public IGuiContainer Parent;
    }
}
