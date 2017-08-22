using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace GuiCookie
{
    public static class ExtensionMethods
    {

        #region COLOUR PARSERS
        public static Color ParseColour(this string colour)
        {
            if (colour.StartsWith("#")) return colour.HexStringToColour();
            else if (colour.Split(',').Length == 3) return colour.RGBStringToColour();
            else return Color.Honeydew;
        }

        private static Color HexStringToColour(this string hexCode)
        {
            //Split the string into 3, for red, green and blue, ignoring the first character which is a hash
            int valueLength = (hexCode.Length - 1) / 3;
            string hexRed = hexCode.Substring(1, valueLength);
            string hexGreen = hexCode.Substring(1 + valueLength, valueLength);
            string hexBlue = hexCode.Substring(1 + (valueLength * 2), valueLength);
            int red = int.Parse(hexRed, System.Globalization.NumberStyles.HexNumber);
            int green = int.Parse(hexGreen, System.Globalization.NumberStyles.HexNumber);
            int blue = int.Parse(hexBlue, System.Globalization.NumberStyles.HexNumber);
            return new Color(red, green, blue);
        }

        private static Color RGBStringToColour(this string RGBCode)
        {
            //Split the string up into the 3 values
            string[] values = RGBCode.Split(',');

            //If the string is invalid, return the default colour
            if (values.Length != 3) return Color.Honeydew;

            //Return a colour based off the string
            return new Color(byte.Parse(values[0]), byte.Parse(values[1]), byte.Parse(values[2]));
        }
        #endregion

        public static object ParseFunctionParameter(this string parameterString)
        {
            switch(parameterString[parameterString.Length - 1])
            {
                case 'f':
                    return parseFloatString(parameterString);
                case '\'':
                    return parseStringString(parameterString);
                default:
                    return parseIntString(parameterString);
            }
        }

        private static float parseFloatString(string f)
        {
            //Tries to parse the float
            float parsedFloat;
            bool parseSucceeded = float.TryParse(f.Remove(f.Length - 1), out parsedFloat);

            //Return the parsed float if it was successful
            if (parseSucceeded)
                return parsedFloat;
            //If the string could not be parsed, throw an error
            else
                throw new Exception("Invalid function float parameter. Float parameter must be numbers with at most one decimal point and no letters other than the f suffix.");
        }

        private static int parseIntString(string i)
        {
            //Tries to parse the integer
            int parsedInt;
            bool parsedSucceeded = int.TryParse(i, out parsedInt);

            //Return the parsed int if it was successful
            if (parsedSucceeded)
                return parsedInt;
            //If the string could not be parsed, throw an error
            else
                throw new Exception("Invalid function integer parameter. Integer parameter must be numbers only.");
        }

        private static string parseStringString(string s)
        {
            //Removes any whitespace before the string
            s = s.TrimStart(' ');

            //If the string does not start with a quote, throw an error
            if (!s.StartsWith("'"))
                throw new Exception("Invalid function string parameter. Parameter must start and end with a ' mark.");

            //Remove the start and end of the string and return it
            return s.Remove(s.Length - 1).Remove(0, 1);
        }

        public static Rectangle ParseRectangle(this string rectangleString)
        {
            //Split the string up into the 4 values
            string[] rectangleValues = rectangleString.Split(',');

            //If the string is invalid, return an empty rectangle instead
            if (rectangleValues.Length != 4) return Rectangle.Empty;

            //Return a new rectangle based off the string
            return new Rectangle(int.Parse(rectangleValues[0]), int.Parse(rectangleValues[1]), int.Parse(rectangleValues[2]), int.Parse(rectangleValues[3]));
        }

        public static float RelativeToScalar(this string relativePosition)
        {
            //Checks that the given string is valid
            if (!relativePosition.EndsWith("%")) throw new ArgumentException("Given string does not end with a % symbol.");
            else if (relativePosition.Length <= 1) throw new ArgumentException("Given string is empty or does not begin with a number value.");

            //Parses the string and returns it as a scalar float
            return float.Parse(relativePosition.Substring(0, relativePosition.Length - 1)) / 100;
        }

        public static Vector2 ParseVector2(this XmlAttribute attribute)
        {
            //If the attribute doesn't exist, return a default Vector2
            if (attribute == null) return Vector2.Zero;

            //Get the given parameters for the size
            string[] vectorParams = attribute.Value.Split(',');

            //Handles creating the anchor based on the given axis parameters
            if (vectorParams.Length == 1) return new Vector2(float.Parse(vectorParams[0]));
            else if (vectorParams.Length == 2) return new Vector2(float.Parse(vectorParams[0]), float.Parse(vectorParams[1]));
            else return new Vector2(0);
        }

        public static string ParseString(this XmlAttribute attribute)
        {
            return (attribute == null) ? string.Empty : attribute.Value;
        }

        #region TEXTURE FUNCTIONS
        public static Texture2D GetTexture(this Texture2D Image, Rectangle Source)
        {
            //If no source has been set, return null
            if (Source == Rectangle.Empty) return null;

            //Makes a new array of Color and sets it to the area from the image
            Color[] newData = new Color[Source.Width * Source.Height];
            Image.GetData(0, Source, newData, 0, newData.Length);

            //Makes a new texture based on the data from the section of the supplied image and returns it
            Texture2D newTexture = new Texture2D(Image.GraphicsDevice, Source.Width, Source.Height);
            newTexture.SetData(newData);
            return newTexture;
        }

        public static Texture2D Rotate(this Texture2D texture, int numberOfTurns)
        {
            //The dimensions of the original texture
            int width = texture.Width, height = texture.Height;

            //Create a new array for the colour data, extract it from the image, then make a 2D array from it
            Color[] colourData = new Color[width * height];
            texture.GetData(colourData);
            Color[,] colourMatrix = colourData.to2DArray(width, height);
            
            //Rotate it numberOfTurns
            for (int i = 0; i < numberOfTurns; i++) colourMatrix = colourMatrix.RotateClockWise();
            
            //Make a new texture from the data and return it
            Texture2D returnTexture = new Texture2D(texture.GraphicsDevice, colourMatrix.GetLength(0), colourMatrix.GetLength(1));
            returnTexture.SetData(colourMatrix.to1DArray());
            return returnTexture;
        }
        #endregion


        private static T[] to1DArray<T>(this T[,] array)
        {
            T[] newArray = new T[array.GetLength(0) * array.GetLength(1)];

            for (int x = 0; x < array.GetLength(0); x++)
                for (int y = 0; y < array.GetLength(1); y++)
                    newArray[x + (y * array.GetLength(0))] = array[x, y];

            return newArray;
        }

        private static T[,] to2DArray<T>(this T[] array, int width, int height)
        {
            T[,] newArray = new T[width, height];

            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    newArray[x, y] = array[x + (y * width)];

            return newArray;
        }

        public static T[,] RotateClockWise<T>(this T[,] matrix)
        {
            T[,] ret = new T[matrix.GetLength(1), matrix.GetLength(0)];

            for (int x = 0; x < matrix.GetLength(1); ++x)
                for (int y = 0; y < matrix.GetLength(0); ++y)
                    ret[x, y] = matrix[y, matrix.GetLength(1) - x - 1];

            return ret;
        }

        public static bool DerivesFrom(this Type Child, Type Parent)
        {
            Type checkType = Child;
            while (checkType != typeof(object))
            {
                if (checkType == Parent) return true;
                checkType = checkType.BaseType;
            }

            return false;
        }

        public static int InheritanceLevel(this Type Child, Type Parent)
        {
            int count = 0;
            Type checkType = Child;
            while (checkType != typeof(object))
            {
                if (checkType == Parent) return count;
                count++;
                checkType = checkType.BaseType;
            }

            return -1;
        }
    }
}
