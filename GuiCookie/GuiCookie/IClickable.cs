using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuiCookie
{
    public delegate void OnClicked(string message);

    public interface IClickable
    {
        event OnClicked Clicked;
    }
}
