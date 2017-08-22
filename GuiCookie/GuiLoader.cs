using GuiCookie.Elements;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace GuiCookie
{
    public partial class Root
    {
        private void loadGuiSheet(string guiSheet)
        {
            XmlDocument guiDocument = new XmlDocument();
            guiDocument.Load(guiSheet);

            Elements = loadGuiNode(guiDocument.SelectSingleNode("Main"), this);
        }

        private List<Element> loadGuiNode(XmlNode node, IGuiContainer parent)
        {
            List<Element> elementChildren = new List<Element>(node.ChildNodes.Count);

            foreach (XmlNode childNode in node)
            {
                //The type of the element
                Type elementType = typeof(Element).Assembly.GetType(typeof(Element).Namespace + "." + childNode.Name);

                //Make an object array for the element's constructor argument. All element constructors have the attributes and the parent, so those are added first
                List<object> childArguments = new List<object>(2);
                childArguments.Add(childNode.Attributes);
                childArguments.Add(parent);

                //Currently no element needs extra arguments, in the future there will be a switch(childNode.Name) to add specific arguments to the object array
                switch (childNode.Name)
                {
                    //Add the delegate and object arguments to the arguments of the element
                    case "Button":
                        if (childNode.Attributes["Function"] == null)
                        {
                            childArguments.Add(null);
                            childArguments.Add(null);
                        }
                        else
                        {
                            object[] methodParams = getFunctionParameters(childNode.Attributes["Function"].Value);

                            childArguments.Add(getFunctionDelegate(childNode.Attributes["Function"].Value, methodParams));
                            childArguments.Add(methodParams);
                        }
                        break;
                }

                //Creates an element based on the childNode
                Element childElement = (Element)Activator.CreateInstance(elementType, childArguments.ToArray());

                //Adds this element to the children of the parent
                elementChildren.Add(childElement);

                //If this element is a container, iterate through its children as well
                if (childNode.HasChildNodes && childElement is IGuiContainer f)
                    f.AddElements(loadGuiNode(childNode, f));
            }

            return elementChildren;
        }

        /// <summary> Returns a delegate of the user-defined model class based on a string representing a function and parameters. </summary>
        /// <param name="methodString"> The full string representing the function and its parameters. </param>
        /// <param name="methodParams"> The object array representing the function's parameters. </param>
        /// <returns> A delegate of a user-defined method. </returns>
        private Delegate getFunctionDelegate(string methodString, object[] methodParams)
        {
            //Gets the name of the method
            string methodName = methodString.Split('(')[0];

            //Gets the type of each parameter and puts it all into an array
            Type[] argTypes = new Type[methodParams.Length + 1];
            for (int i = 0; i < methodParams.Length; i++)
                argTypes[i] = methodParams[i].GetType();

            //Sets the last type to void, as that is the delegate's return type
            argTypes[argTypes.Length - 1] = typeof(void);

            //Creates a delegate from the types
            Type delegateType = Expression.GetDelegateType(argTypes);

            //Since this class will be inherited from by the user, we can find the methods they've implemented
            MethodInfo method = GetType().GetMethod(methodName);

            //If they haven't implemented the method, throw an error
            if (method == null)
                throw new Exception("Function was defined in the GuiSheet but not in the model class.");

            //Returns the delegate, based on the type, targeting this class, and using the found method
            try
            {
                return Delegate.CreateDelegate(delegateType, this, method);
            }
            //Catches the ArgumentException and throws a more specific Exception, then returns null
            catch (Exception e)
            {
                if (e is ArgumentException)
                    throw new Exception("Function defined in the GuiSheet does not match function defined in the model class. Check that the parameters match.");
                return null;
            }
        }

        private object[] getFunctionParameters(string argumentString)
        {
            //Throws an error if the function string is incorrect
            if (!argumentString.Contains("(") || !argumentString.EndsWith(")"))
                throw new Exception("Function defined in the GuiSheet is malformed. Function should have a \"(\" symbol between the function name and parameters, and a \")\" symbol on the end.");

            //Splits the string into two parts, the function name, and the function parameters
            string argString = argumentString.Split('(')[1];

            //removes the end ) from the string
            argString = argString.Remove(argString.Length - 1);

            //Splits the string into the individual arguments
            string[] stringArguments = argString.Split(',');

            //Goes through each argument and parses it to an object
            object[] functionArguments = new object[stringArguments.Length];
            for (int i = 0; i < stringArguments.Length; i++)
                functionArguments[i] = stringArguments[i].ParseFunctionParameter();

            //Returns the final arguments
            return functionArguments;
        }
    }
}
