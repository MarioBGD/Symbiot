using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbiot.Portable.Source.Circles;

namespace Symbiot.Portable.Source.Controllers
{
    public class GameController
    {
        private CameraController cameraController;
        private CirclesManager circlesManager;

        public GameController()
        {
            cameraController = new CameraController();
            circlesManager = new CirclesManager();
        }

        public void OnDraw(SpriteBatch spriteBatch)
        {
            circlesManager.OnDraw(spriteBatch);
        }
    }
}
