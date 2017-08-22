using GuiCookie;
using GuiCookie.Elements;
using Microsoft.Xna.Framework.Content;
using System;

namespace TestProject
{
    public class TestModel : Root
    {
        Button Button;
        Random rnd = new Random();


        public TestModel(string guiSheet,string styleSheet, ContentManager content)
            : base (guiSheet, styleSheet, content)
        {
            Button = GetElementByID("Button1") as Button;
        }

        public void TestMethod(float five, int two, string hello)
        {
            float ten = five * 2f;
        }

        public override void Update()
        {
            base.Update();

            Button.SetText.Invoke(rnd.Next().ToString());
        }
    }
}
