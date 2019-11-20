using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGCore.UI
{
    public abstract class UIElement
    {
        private bool enabled = true;
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (value != enabled)
                {
                    if (value) UIManager.Instance.Elements.Add(this);
                    else UIManager.Instance.Elements.Remove(this);
                    enabled = value;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void Dispose()
        {
            
        }
    }
}
