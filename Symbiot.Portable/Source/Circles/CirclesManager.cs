using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Circles
{
    public class CirclesManager
    {
        private CirclesPainter painter;
        private CirclesGenerator generator;

        public List<Vector2> Circles = new List<Vector2>()

        public CirclesManager()
        {
            painter = new CirclesPainter();
            generator = new CirclesGenerator();
        }

        public void OnDraw(SpriteBatch spriteBatch)
        {
            painter.OnDraw(spriteBatch);
        }
    }
}
