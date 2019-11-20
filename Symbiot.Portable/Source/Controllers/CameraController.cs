using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Controllers
{
    class CameraController
    {
        public Vector2 Position { get; private set; } = new Vector2(0);
        private Vector2 goToPos = new Vector2(0);

        public CameraController()
        {
            GameRoot.Instance.OnUpdate += OnUpdate;
        }

        public void GoTo(Vector2 pos)
        {
            goToPos = pos;
        }

        public void OnUpdate(GameTime gameTime)
        {
            Position = Vector2.Lerp(Position, goToPos, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    }
}
