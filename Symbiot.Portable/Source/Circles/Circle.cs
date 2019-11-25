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
    public class Circle
    {
        public static Texture2D CircleTexture;
        public static Vector2 CircleSize;
        public const float HitboxMultiplier = 1.4f;
        public const float circleRadius = 1f;

        public Vector2 Position;

        public Circle(Vector2 pos)
        {
            Setup(pos);
        }

        public void Setup(Vector2 pos)
        {
            Position = pos;
        }

        public void OnDraw(SpriteBatch spriteBatch)
        {
            if (Position.X < CameraController.Instance.LeftRenderBorder
                || Position.X > CameraController.Instance.RightRenderBorder
                || Position.Y > CameraController.Instance.TopRenderBorder)
                return;

            spriteBatch.Draw(CircleTexture, new Rectangle(
                CameraController.Instance.WorldPosToPixels(Position - new Vector2(circleRadius, -circleRadius)),
                new Point(CameraController.Instance.WorldLenghtToPixels(circleRadius * 2))),
                Color.White);
        }
    }
}
