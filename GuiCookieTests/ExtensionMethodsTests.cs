using GuiCookie;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using System;

namespace GuiCookie.Tests
{
    [TestClass()]
    public class ExtensionMethodsTests
    {
        [TestMethod()]
        public void FiftyTest()
        {
            Assert.AreEqual(0.5f, "50%".RelativeToScalar());
        }

        [TestMethod()]
        public void OnehundredAndTenTest()
        {
            Assert.AreEqual(1.1f, "110%".RelativeToScalar());
        }

        [TestMethod()]
        public void NoPercentageTest()
        {
            Assert.ThrowsException<ArgumentException>(() => "50".RelativeToScalar());
        }

        [TestMethod()]
        public void OnlyPercentageTest()
        {
            Assert.ThrowsException<ArgumentException>(() => "%".RelativeToScalar());
        }

        [TestMethod()]
        public void EmptyStringTest()
        {
            Assert.ThrowsException<ArgumentException>(() => string.Empty.RelativeToScalar());
        }

        [TestMethod()]
        public void ParseBlackTest()
        {
            Assert.AreEqual(Color.Black, "0, 0, 0".ParseColour());
        }

        [TestMethod()]
        public void ParseWhiteTest()
        {
            Assert.AreEqual(Color.White, "255, 255, 255".ParseColour());
        }

        [TestMethod()]
        public void ParseGreenTest()
        {
            Assert.AreEqual(Color.Green, "0, 128, 0".ParseColour());
        }
    }
}