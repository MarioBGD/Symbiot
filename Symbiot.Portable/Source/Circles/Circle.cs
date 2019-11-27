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
        private const float outOfRadius = 0.8f;

        public Vector2 Position;
        private float currentRadius = outOfRadius;

        public Circle(Vector2 pos)
        {
            Setup(pos);
        }

        public void Setup(Vector2 pos)
        {
            Position = pos;
            currentRadius = (IsInRange()) ? circleRadius : outOfRadius;
        }

        public void OnDraw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            if (Position.X < CameraController.Instance.LeftRenderBorder
                || Position.X > CameraController.Instance.RightRenderBorder
                || Position.Y > CameraController.Instance.TopRenderBorder)
                return;

            float toRadius = (IsInRange()) ? circleRadius: outOfRadius;
               
            if (toRadius != currentRadius)
            {
                if (currentRadius < toRadius)
                {
                    currentRadius += (float)gameTime.ElapsedGameTime.TotalSeconds * 2.2f;
                    if (currentRadius > toRadius)
                        currentRadius = toRadius;
                }
                else
                {
                    currentRadius -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.2f;
                    if (currentRadius < toRadius)
                        currentRadius = toRadius;
                }
            }

            spriteBatch.Draw(CircleTexture, new Rectangle(
                CameraController.Instance.WorldPosToPixels(Position - new Vector2(currentRadius, -currentRadius)),
                new Point(CameraController.Instance.WorldLenghtToPixels(currentRadius * 2))),
                Color.White);
        }

        public bool IsInRange()
        {
            return (Position.Y > AreaController.Position.Y
                && Vector2.Distance(AreaController.Position, Position) <= AreaController.Radius + circleRadius * HitboxMultiplier);
        }

        private float Lerp(float val, float toVal, float t)
        {
            return val + (toVal - val) * t;
        }
    }
}
