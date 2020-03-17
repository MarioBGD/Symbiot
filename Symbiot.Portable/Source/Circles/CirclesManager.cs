using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Symbiot.Portable.Source.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Circles
{
    public class CirclesManager
    {
        private CirclesGenerator generator;
        private AreaController areaController;
      
        public List<Circle> EnabledCircles = new List<Circle>();
        public List<Circle> DisabledCircles = new List<Circle>();

        private bool Generating = true;
        private Circle CurrentCircle;

        public CirclesManager(AreaController ac)
        {
            generator = new CirclesGenerator();
            areaController = ac;

            GameRoot.Instance.OnLoad += (Game gc) =>
            {
                Circle.CircleTexture = gc.Content.Load<Texture2D>("Sprites/Game/Circle");
            };
            GameRoot.Instance.OnDraw += OnDraw;
            InputController.Instance.OnClick += OnClick;

            Start();
        }

        public void Stop()
        {
            GameRoot.Instance.OnUpdate -= OnUpdate;
        }


        public void Start()
        {
            foreach (Circle c in EnabledCircles)
                DisabledCircles.Add(c);
            EnabledCircles.Clear();

            generator.GenerateCircles(ref EnabledCircles, ref DisabledCircles, 7);
            CurrentCircle = generator.GetFromGrid(new Point(0));
            Vector2 a = CurrentCircle.Position;

            areaController.Start(CurrentCircle.Position);

            GameRoot.Instance.OnUpdate += OnUpdate;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (CirclesGenerator.PosToIndex(new Vector2(0, CameraController.Instance.TopRenderBorder)).Y > generator.lastGeneratedIndex)
            {
                generator.GenerateCircles(ref EnabledCircles, ref DisabledCircles, 7);
            }
        }

        private void CleanUslessCircle()
        {
            for (int i = 0; i < EnabledCircles.Count; i++)
                if (EnabledCircles[i].Position.Y < CameraController.Instance.BotRenderBorder)
                {
                    DisabledCircles.Add(EnabledCircles[i]);
                    EnabledCircles.RemoveAt(i);
                    break;
                }
        }

        public void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            Point leftBot = CirclesGenerator.PosToIndex(new Vector2(CameraController.Instance.LeftRenderBorder, CameraController.Instance.BotRenderBorder));
            Point rightTop = CirclesGenerator.PosToIndex(new Vector2(CameraController.Instance.RightRenderBorder, CameraController.Instance.TopRenderBorder));

            for (int x = leftBot.X; x <= rightTop.X; x++)
                for (int y = leftBot.Y; y <= rightTop.Y; y++)
                {
                    Circle c = generator.GetFromGrid(new Point(x, y));
                    if (c != null)
                        c.OnDraw(spriteBatch, gameTime, x);
                }
        }

        public void OnClick(Point point)
        {
            if (!Generating)
                return;

            Vector2 worldPos = CameraController.Instance.PixelsToWorldPos(point);
            Point index = CirclesGenerator.PosToIndex(worldPos);

            for (int x = -1; x <= 1; x++)
            {
                int ix = index.X + x;

                for (int y = -1; y <= 1; y++)
                {
                    int iy = index.Y + y;
                    Circle c = generator.GetFromGrid(new Point(ix, iy));

                    if (c == null) continue;
                    //EnabledCircles[i].Position.Y > CurrentCircle.Position.Y
                    Vector2 cGlobalPos = c.GlobalPos(ix);
                    if (Vector2.Distance(cGlobalPos, worldPos) < Circle.circleRadius * Circle.HitboxMultiplier
                        && Vector2.Distance(cGlobalPos, AreaController.Position) < AreaController.Radius + Circle.circleRadius)
                    {
                        CircleClicked(c, cGlobalPos);
                        break;
                    }
                }
            }
        }

        private void CircleClicked(Circle circle, Vector2 worldPos)
        {
            CurrentCircle = circle;
            CameraController.Instance.GoTo(worldPos);
            areaController.Reset(worldPos);
        }
    }
}
