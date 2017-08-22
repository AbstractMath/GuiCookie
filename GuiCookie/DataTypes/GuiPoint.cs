using System.Xml;

namespace GuiCookie.DataTypes
{
    public struct GuiPoint
    {
        #region PRIVATE PROPERTIES
        /// <summary> A blank GuiPoint </summary>
        private static GuiPoint EmptyPoint { get => new GuiPoint() { X = 0, Y = 0, RelativeX = false, RelativeY = false }; }
        #endregion

        #region PUBLIC PROPERTIES
        /// <summary> The X axis. </summary>
        public float X { get; private set; }

        /// <summary> The Y axis. </summary>
        public float Y { get; private set; }

        /// <summary> Whether or not the X axis is relative. </summary>
        public bool RelativeX { get; private set; }

        /// <summary> Whether or not the Y axis is relative. </summary>
        public bool RelativeY { get; private set; }
        #endregion

        #region INTERNAL CONSTRUCTORS
        /// <summary> Creates a new GuiPoint from the given XmlAttribute. </summary>
        /// <param name="attribute"> The XmlAttribute to parse. </param>
        internal GuiPoint(XmlAttribute attribute)
        {
            //If the attribute is null, default to an empty point
            if (attribute == null) { this = EmptyPoint; return; }
            
            //Split the attribute into values
            string[] pointValues = attribute.Value.Split(',');

            //If there's only one value, use this for both axes
            if (pointValues.Length == 1)
                this = new GuiPoint(pointValues[0], pointValues[0]);
            //If there's two values, use them for the axes
            else if (pointValues.Length == 2)
                this = new GuiPoint(pointValues[0], pointValues[1]);
            //If there's zero or more than two values, default to empty
            else this = EmptyPoint;
        }
        #endregion

        #region PUBLIC CONSTRUCTORS
        /// <summary> Creates a new GuiPoint with the given string values, allowing relative values, e.g. "100%". </summary>
        /// <param name="x"> The X axis. </param>
        /// <param name="y"> The Y axis. </param>
        public GuiPoint(string x, string y)
        {
            RelativeX = x.EndsWith("%");
            RelativeY = y.EndsWith("%");

            X = RelativeX ? x.RelativeToScalar() : float.Parse(x);
            Y = RelativeY ? y.RelativeToScalar() : float.Parse(y);
        }

        /// <summary> Creates a new GuiPoint with the given int values, meaning it is unable to be relative. </summary>
        /// <param name="x"> The X axis. </param>
        /// <param name="y"> The Y axis. </param>
        public GuiPoint(int x, int y)
        {
            //Set the X and Y to the given values
            X = x;
            Y = y;

            //Since these are int values, they are unable to be relative
            RelativeX = false;
            RelativeY = false;
        }

        /// <summary> Create a new GuiPoint with the given axes and relatives. </summary>
        /// <param name="x"> The X axis. </param>
        /// <param name="y"> The Y axis. </param>
        /// <param name="rx"> The X relativity. </param>
        /// <param name="ry"> The Y relativity. </param>
        public GuiPoint(float x, float y, bool rx, bool ry)
        {
            //Set the axes
            X = x;
            Y = y;

            //Set the relatives
            RelativeX = rx;
            RelativeY = ry;
        }
        #endregion
    }
}
