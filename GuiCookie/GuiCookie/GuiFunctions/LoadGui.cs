using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Xml;

namespace GuiCookie
{
    public static partial class GuiFunctions
    {
        public static GuiData LoadGuiSheet(string FileName, StyleSettings StyleSettings, OnClicked ButtonPressed)
        {
            //Attempts to load the document
            XmlDocument sheetDocument = new XmlDocument();
            try
            {
                sheetDocument.Load(FileName);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error '{0}' occured", e);
                return new GuiData();
            }
            //Everything in the GuiSheet
            XmlNode mainNode = sheetDocument.GetElementsByTagName("Main")[0];
            //Makes the element list to be returned
            List<Element> finalElements = new List<Element>(mainNode.ChildNodes.Count);

            finalElements = EnumerateNode(mainNode, null, StyleSettings, ButtonPressed);
            GuiData data = new GuiData();
            data.Elements = finalElements;
            data.DesignResolution = new Vector2(Convert.ToInt16(mainNode.Attributes.GetNamedItem("ResolutionX").Value), Convert.ToInt16(mainNode.Attributes.GetNamedItem("ResolutionY").Value));
            return data;
        }

        private static List<Element> EnumerateNode(XmlNode Node, IGuiContainer Parent, StyleSettings StyleSettings, OnClicked ButtonPressed)
        {
            List<Element> finalElements = new List<Element>(Node.ChildNodes.Count);

            foreach (XmlNode n in Node.ChildNodes)
            {
                Element element = GetElementFromNode(n, Parent, StyleSettings, ButtonPressed);
                finalElements.Add(element);
                if (n.HasChildNodes)
                    finalElements.AddRange(EnumerateNode(n, element, StyleSettings, ButtonPressed));

            }


            return finalElements;
        }

        private static Element GetElementFromNode(XmlNode Node, IGuiContainer Parent, StyleSettings StyleSettings, OnClicked ButtonPressed)
        {
            //Gets the Position, Size and Offset which are common between all elements

            XmlAttributeCollection nodeAttributes = Node.Attributes;
            ElementTemplate baseTemplate = CreateElementTemplate(nodeAttributes, Parent);
            switch (Node.Name)
            {
                case "Button":
                    Button b = new Button(CreateButtonTemplate(nodeAttributes, baseTemplate, StyleSettings));
                    b.Clicked += ButtonPressed;
                    return b;
                case "Frame":
                    return new Frame(CreateFrameTemplate(nodeAttributes, baseTemplate));
                case "TextBlock":
                    return new TextBlock(CreateTextBlockTemplate(nodeAttributes, baseTemplate, StyleSettings));
            }
            return null;
        }
    }
}
