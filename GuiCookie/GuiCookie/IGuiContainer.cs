using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuiCookie
{
    public interface IGuiContainer
    {
        Element GetNamedElement(string Name);
        Vector2 Position { get; }
    }
}
