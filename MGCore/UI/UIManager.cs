using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MGCore.UI
{
    public class UIManager
    {
        public static UIManager Instance;

        public List<UIElement> Elements = new List<UIElement>();

        public UIManager()
        {
            Instance = this;
        }


        public void OnDraw(SpriteBatch spriteBatch)
        {
            foreach (UIElement element in Elements)
            {
                element.Draw(spriteBatch);
            }
        }
    }
}
