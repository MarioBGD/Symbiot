using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Symbiot.Portable.Source.Controllers
{
    class CameraController
    {
        public static CameraController Instance;

        public Vector2 CameraSize { get; private set; }
        public Vector2 CameraRadius { get; private set; }
        public Vector2 ScreenPixelSzie { get; private set; }
        public Vector2 Position { get; private set; } = new Vector2(0);

        public Vector2 RenderPosition => Position + CameraOffset;
        private Vector2 CameraOffset;

        private Vector2 goToPos = new Vector2(0);
        private float linearSpeed = 30; //[m/s]
        private float lerpSpeed = 12; //[m/s]
        private bool linear = false;

        public CameraController()
        {
            
            Instance = this;
            GameRoot.Instance.OnUpdate += OnUpdate;
            ScreenPixelSzie = new Vector2(GameRoot.Instance.GraphicsDevice.Viewport.Width,
                GameRoot.Instance.GraphicsDevice.Viewport.Height);
            CameraSize = new Vector2(ScreenPixelSzie.X / ScreenPixelSzie.Y * 20, 20);
            CameraRadius = CameraSize / 2;

            CameraOffset = new Vector2(0, CameraRadius.Y * 0.5f);
        }

        public void GoTo(Vector2 pos)
        {
            goToPos = pos;
        }

        public void OnUpdate(GameTime gameTime)
        {
            if (linear)
            {
                if (Position != goToPos)
                {
                    float stepBy = linearSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (Vector2.Distance(Position, goToPos) < stepBy)
                        Position = goToPos;
                    else
                    {
                        Vector2 dir = Vector2.Normalize(goToPos - Position);
                        Position += dir * stepBy;
                    }
                }
            }
            else
            Position = Vector2.Lerp(Position, goToPos, (float)gameTime.ElapsedGameTime.TotalSeconds * lerpSpeed);
        }

        public float LeftRenderBorder => Instance.RenderPosition.X - (Instance.CameraSize.X * 0.72f);
        public float RightRenderBorder => Instance.RenderPosition.X + (Instance.CameraSize.X * 0.72f);
        public float TopRenderBorder => Instance.RenderPosition.Y + (Instance.CameraSize.Y * 0.5f);
        public float BotRenderBorder => Instance.RenderPosition.Y - (Instance.CameraSize.Y * 0.61f);

        public Point WorldPosToPixels(Vector2 pos)
        {
            return new Point(
                (int)map(pos.X, RenderPosition.X - CameraRadius.X, RenderPosition.X + CameraRadius.X, 0, ScreenPixelSzie.X),
                (int)map(pos.Y, RenderPosition.Y - CameraRadius.Y, RenderPosition.Y + CameraRadius.Y, ScreenPixelSzie.Y, 0));
        }

        public Vector2 PixelsToWorldPos(Point point)
        {
            return new Vector2(
                map(point.X, 0, ScreenPixelSzie.X, RenderPosition.X - CameraRadius.X, RenderPosition.X + CameraRadius.X),
                map(point.Y, ScreenPixelSzie.Y, 0, RenderPosition.Y - CameraRadius.Y, RenderPosition.Y + CameraRadius.Y));
        }

        public int WorldLenghtToPixels(float lenght)
        {
            return (int)map(lenght, 0, CameraSize.X, 0, ScreenPixelSzie.X);
        }

        private float map(float val, float from1, float from2, float to1, float to2)
        {
            return to1 + (to2 - to1) * ((val - from1) / (from2 - from1));
        }
    }
}
