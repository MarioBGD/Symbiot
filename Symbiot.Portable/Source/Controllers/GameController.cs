using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symbiot.Portable.Source.Circles;
using Microsoft.Xna.Framework;

namespace Symbiot.Portable.Source.Controllers
{
    public class GameController
    {
        private CameraController cameraController;
        private CirclesManager circlesManager;
        private AreaController areaController;

        public GameController()
        {
            cameraController = new CameraController();
            areaController = new AreaController(this);
            circlesManager = new CirclesManager(areaController);
            //StartGame();
        }

        public void StartGame()
        {
            circlesManager.Start();
            //areaController is starting in circlesManager.Start()
        }

        public void FinishGame()
        {
            circlesManager.Stop();
            areaController.Stop();
        }

        public void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            circlesManager.OnDraw(spriteBatch, gameTime);
        }
    }
}
