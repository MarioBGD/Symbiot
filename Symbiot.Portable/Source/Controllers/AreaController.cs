using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Controllers
{
    public class AreaController
    {
        private const float startRadius = 8;
        private const float startSpeed = 2f; //[m/s]
        private const float acceleration = 0.01f; //[m/s]

        private static Texture2D AreaCircleTexture;
        private static AreaController Instance;
        private GameController gameController;
        
        public static float Radius { get; private set; }
        private float Speed;
        public static Vector2 Position { get; private set; }
        

        public AreaController(GameController gc)
        {
            Instance = this;
            gameController = gc;

            GameRoot.Instance.OnLoad += (Game game) =>
            {
                AreaCircleTexture = game.Content.Load<Texture2D>("Sprites/Game/Area");
            };
        }

        public void Start(Vector2 pos)
        {
            Speed = startSpeed;
            Radius = startRadius;
            Position = pos;

            GameRoot.Instance.OnUpdate += OnUpdate;
            GameRoot.Instance.OnDraw += OnDraw;
        }

        public void Reset(Vector2 pos)
        {
            Radius = startRadius;
            Position = pos;
        }

        public void Stop()
        {
            GameRoot.Instance.OnUpdate -= OnUpdate;
            GameRoot.Instance.OnDraw -= OnDraw;
        }

        public void OnUpdate(GameTime gameTime)
        {
            Radius -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //if (Radius <= 0)
            //    gameController.FinishGame();

            Speed += acceleration * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(AreaCircleTexture, new Rectangle(
                CameraController.Instance.WorldPosToPixels(Position - new Vector2(Radius, -Radius)),
                new Point(CameraController.Instance.WorldLenghtToPixels(Radius * 2))),
                Color.Green);
        }
    }
}
