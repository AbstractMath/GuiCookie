using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiCookie
{
    public struct StyleSettings
    {
        public Dictionary<string, SpriteFont> Fonts;
        public Dictionary<string, BoxStyle> ButtonStyles;
    }

    public struct BoxStyle
    {
        public Texture2D EdgeTexture { get; set; }
        public Texture2D CornerTexture { get; set; }
        public Texture2D FillTexture { get; set; }
        public bool Tiled { get; set; }
        public SpriteFont Font { get; set; }
    }
}
