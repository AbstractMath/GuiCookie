using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GuiCookie
{
    public static partial class GuiFunctions
    {
        private static Rectangle GetRectangleFromNode(XmlNode Node)
        {
            string[] data = new string[4];
            data = Node.Value.Split(',');
            return new Rectangle(Convert.ToInt16(data[0]), Convert.ToInt16(data[1]), Convert.ToInt16(data[2]), Convert.ToInt16(data[3]));
        }

        private static Vector2 GetVector2FromNode(XmlNode Node)
        {
            string[] data = new string[2];
            data = Node.Value.Split(',');
            return new Vector2(Convert.ToInt16(data[0]), Convert.ToInt16(data[1]));
        }
    }
}
