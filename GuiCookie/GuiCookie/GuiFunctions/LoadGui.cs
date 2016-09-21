using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace GuiCookie
{
    public static partial class GuiFunctions
    {
        public static void LoadGuiSheet(string FileName, StyleSettings StyleSettings, OnClicked ButtonPressed, IGuiContainer MainContainer)
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
                return;
            }
            //Everything in the GuiSheet
            XmlNode mainNode = sheetDocument.GetElementsByTagName("Main")[0];
            EnumerateNode(mainNode, MainContainer, StyleSettings, ButtonPressed);
        }

        //Enumerates through a node and adds its children
        private static void EnumerateNode(XmlNode Node, IGuiContainer Parent, StyleSettings StyleSettings, OnClicked ButtonPressed)
        {
            //Goes through each child node in the parent node
            foreach (XmlNode childNode in Node.ChildNodes)
            {
                //Turns the child node into an element and adds it to the parent element
                Element childElement = GetElementFromNode(childNode, Parent, StyleSettings, ButtonPressed);
                Parent.AddElement(childElement);
                //If the child node has any children, call back on this method
                if (childNode.HasChildNodes)
                {
                    //Check if the child node is actually able to hold children, display an error if it can't
                    if (IsContainerNode(childNode))
                        EnumerateNode(childNode, childElement as IGuiContainer, StyleSettings, ButtonPressed);
                    else
                        Console.WriteLine("Cannot add child nodes to a non-container node!");
                }
            }
        }

        private static Element GetElementFromNode(XmlNode Node, IGuiContainer Parent, StyleSettings StyleSettings, OnClicked ButtonPressed, List<Element> Children = null)
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
                    return new Frame(baseTemplate);
                case "TextBlock":
                    return new TextBlock(CreateTextBlockTemplate(nodeAttributes, baseTemplate, StyleSettings));
            }
            return null;
        }

        private static List<string> ContainerNames = new List<string>
        {
            "Frame"
        };
        private static bool IsContainerNode(XmlNode Node)
        {
            return ContainerNames.Contains(Node.Name, StringComparer.OrdinalIgnoreCase);
        }
    }
}
