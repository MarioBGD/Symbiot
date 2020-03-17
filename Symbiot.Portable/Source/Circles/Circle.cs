using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Symbiot.Portable.Source.Controllers;
using Symbiot.Portable.Source.Effects;
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
        public EffectType EffType { get; private set; }
        private float currentRadius = outOfRadius;
        private Color color = Color.White;

        public Circle(Vector2 pos, EffectType effectType = EffectType.None)
        {
            Setup(pos, effectType);
        }

        public void Setup(Vector2 pos, EffectType effectType = EffectType.None)
        {
            Position = pos;
            EffType = effectType;
            currentRadius = (IsInRange()) ? circleRadius : outOfRadius;

            if (effectType == EffectType.None)
                color = Color.White;
            else if (effectType == EffectType.Freeze)
                color = Color.Cyan;
        }

        public void OnDraw(SpriteBatch spriteBatch, GameTime gameTime, int indexX)
        {
            Vector2 Pos = GlobalPos(indexX);
            /*if (Pos.X < CameraController.Instance.LeftRenderBorder
                || Pos.X > CameraController.Instance.RightRenderBorder
                || Pos.Y > CameraController.Instance.TopRenderBorder)
                return;*/

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
                CameraController.Instance.WorldPosToPixels(Pos - new Vector2(currentRadius, -currentRadius)),
                new Point(CameraController.Instance.WorldLenghtToPixels(currentRadius * 2))),
                color);
        }

        public Vector2 GlobalPos(int indexX)
        {
            return new Vector2(Position.X + (float)Math.Floor((double)indexX / CirclesGenerator.gridSize.X) * CirclesGenerator.cellSize * CirclesGenerator.gridSize.X, Position.Y);
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
