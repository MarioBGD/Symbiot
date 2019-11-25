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
        private Circle Currentcircle;

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

            Currentcircle = generator.GenerateCircle(CameraController.Instance.Position, ref DisabledCircles);
            EnabledCircles.Add(Currentcircle);
            areaController.Start(Currentcircle.Position);

            GameRoot.Instance.OnUpdate += OnUpdate;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (generator.Finished)
            {
                generator.BeginGenerating(ref EnabledCircles);

            }
            else
            {
                Circle c = generator.GenerateCircle(ref EnabledCircles, ref DisabledCircles);
                if (c != null) EnabledCircles.Add(c);

                if (!generator.Finished)
                {
                    c = generator.GenerateCircle(ref EnabledCircles, ref DisabledCircles);
                    if (c != null) EnabledCircles.Add(c);
                }
            }

            CleanUslessCircle();
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

        public void OnDraw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < EnabledCircles.Count; i++)
                EnabledCircles[i].OnDraw(spriteBatch);
        }

        public void OnClick(Point point)
        {
            if (!Generating)
                return;

            Debug.WriteLine(point);
            Vector2 pos = CameraController.Instance.PixelsToWorldPos(point);

            for (int i = 0; i < EnabledCircles.Count; i++)
                if (Vector2.Distance(EnabledCircles[i].Position, pos) < Circle.circleRadius * Circle.HitboxMultiplier)
                {
                    if (EnabledCircles[i].Position.Y > Currentcircle.Position.Y &&
                        Vector2.Distance(EnabledCircles[i].Position, areaController.Position) < areaController.Radius + Circle.circleRadius)
                        CircleClicked(EnabledCircles[i]);
                    break;
                }
        }

        private void CircleClicked(Circle circle)
        {
            Currentcircle = circle;
            CameraController.Instance.GoTo(circle.Position);
            areaController.Reset(circle.Position);
        }
    }
}
