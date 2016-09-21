using System;
using System.Xml;

namespace GuiCookie
{
    public static partial class GuiFunctions
    {
        private static ElementTemplate CreateElementTemplate(XmlAttributeCollection Node, IGuiContainer Parent)
        {
            //Makes a new ElementTemplate
            ElementTemplate template = new ElementTemplate();
            //Sets the Bounding
            template.Bounding = GetRectangleFromNode(Node.GetNamedItem("Bounding"));
            template.Bounding.Location += Parent.Position.ToPoint();
            //Sets the visibility, if none is found it defaults to visible
            XmlNode visible = Node.GetNamedItem("Visible");
            template.Visible = (visible == null) ? true : Convert.ToBoolean(visible.Value);
            //Sets the name, if there is no name it sets it to be empty
            XmlNode name = Node.GetNamedItem("Name");
            template.Name = (name == null) ? string.Empty : name.Value;
            template.Parent = Parent;
            return template;
        }

        private static ButtonTemplate CreateButtonTemplate(XmlAttributeCollection Node, ElementTemplate baseTemplate, StyleSettings styleSettings)
        {
            //Makes a new ButtonTemplate based off the baseTemplate
            ButtonTemplate template = new ButtonTemplate();
            template.Bounding = baseTemplate.Bounding;
            template.Name = baseTemplate.Name;
            template.Visible = baseTemplate.Visible;
            template.Parent = baseTemplate.Parent;
            //Sets the message, if there is no message it sets it to be empty
            XmlNode message = Node.GetNamedItem("Message");
            template.Message = (message == null) ? string.Empty : message.Value;
            //Sets the text, if there is no text it sets it to be empty
            XmlNode text = Node.GetNamedItem("Text");
            template.Text = (text == null) ? string.Empty : text.Value;
            //Sets the buttonstyle
            template.ButtonStyle = styleSettings.ButtonStyles[Node.GetNamedItem("Style").Value];
            return template;
        }

        private static TextBlockTemplate CreateTextBlockTemplate(XmlAttributeCollection Node, ElementTemplate baseTemplate, StyleSettings styleSettings)
        {
            //Makes a new TextBlockTemplate based off the baseTemplate
            TextBlockTemplate template = new TextBlockTemplate();
            template.Bounding = baseTemplate.Bounding;
            template.Name = baseTemplate.Name;
            template.Visible = baseTemplate.Visible;
            template.Parent = baseTemplate.Parent;
            //Sets the text, if there is no text it sets it to be empty
            XmlNode text = Node.GetNamedItem("Text");
            template.Text = (text == null) ? string.Empty : text.Value;
            //Sets the TextBlockStyle
            template.TextBlockStyle = styleSettings.ButtonStyles[Node.GetNamedItem("Style").Value];
            return template;
        }

    }
}
