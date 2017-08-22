using Microsoft.Xna.Framework;
using System.Xml;

namespace GuiCookie.DataTypes
{
    public struct Padding
    {
        #region PRIVATE FIELDS
        private float[] sides;
        private bool[] relatives;
        #endregion

        #region PUBLIC PROPERTIES
        public float Left { get => sides[0]; }
        public float Top { get => sides[1]; }
        public float Right { get => sides[2]; }
        public float Bottom { get => sides[3]; }

        public bool RelativeLeft { get => relatives[0]; }
        public bool RelativeTop { get => relatives[1]; }
        public bool RelativeRight { get => relatives[2]; }
        public bool RelativeBottom { get => relatives[3]; }
        #endregion

        #region PUBLIC CONSTRUCTORS
        internal Padding(XmlAttribute attribute)
        {
            
            if (attribute == null) { this = new Padding("0", "0", "0", "0"); return; }

            string[] paddingValues = attribute.Value.Split(',');

            if (paddingValues.Length == 1) this = new Padding(paddingValues[0], paddingValues[0], paddingValues[0], paddingValues[0]);
            else if (paddingValues.Length == 2) this = new Padding(paddingValues[0], paddingValues[1], paddingValues[0], paddingValues[1]);
            else if (paddingValues.Length == 4) this = new Padding(paddingValues[0], paddingValues[1], paddingValues[2], paddingValues[3]);
            else this = new Padding("0", "0", "0", "0");
            
        }

        /// <summary> Creates a new padding with the given string values, allowing relative values, e.g. "100%". </summary>
        /// <param name="left"> The left padding. </param>
        /// <param name="top"> The top padding. </param>
        /// <param name="right"> The right padding. </param>
        /// <param name="bottom"> The bottom padding. </param>
        public Padding (string left, string top, string right, string bottom)
        {
            relatives = new bool[4]
            {
                left.EndsWith("%"),
                top.EndsWith("%"),
                right.EndsWith("%"),
                bottom.EndsWith("%")
            };

            sides = new float[4]
            {
                relatives[0] ? left.RelativeToScalar() : float.Parse(left),
                relatives[1] ? top.RelativeToScalar() : float.Parse(top),
                relatives[2] ? right.RelativeToScalar() : float.Parse(right),
                relatives[3] ? bottom.RelativeToScalar() : float.Parse(bottom)
            };
        }
        #endregion
    }
}
