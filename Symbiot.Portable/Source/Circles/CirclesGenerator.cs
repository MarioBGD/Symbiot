using Microsoft.Xna.Framework;
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
    public class CirclesGenerator
    {
        public static  readonly Point gridSize = new Point(9, 26);
        public static float cellSize;

        private const int k = 10;
        private const float r = 4f;

        private float freezeChance = 0.01f;

        public Circle[] grid { private set; get; }

        public int lastGeneratedIndex = 0;

        private List<Circle> activeCircles = new List<Circle>();
        private Random random;

        public bool Finished => (activeCircles.Count == 0);

        public CirclesGenerator()
        {
            cellSize = r / (float)Math.Sqrt(2);
            grid = new Circle[gridSize.X * gridSize.Y];
            //grid[0] = new Circle(Vector2.Zero + Vector2.One * 0.3f);
            random = new Random();
        }

        private void SetInGrid(Point index, Circle circle)
        {
            Point i = FixedIndex(index);
            grid[gridSize.X * i.Y + i.X] = circle;
        }

        public Circle GetFromGrid(Point index)
        {
            Point i = FixedIndex(index);
            return grid[gridSize.X * i.Y + i.X];
        }

        public static Point PosToIndex(Vector2 pos)
        {
            return new Point((int)Math.Floor(pos.X / cellSize), (int)Math.Floor(pos.Y / cellSize));
        }

        private Point FixedIndex(Point index)
        {
            return new Point(mod(index.X, gridSize.X), mod(index.Y, gridSize.Y));
        }

        private Vector2 FixedPos(Vector2 pos)
        {
            return new Vector2(mod(pos.X, (gridSize.X * cellSize)), pos.Y);
        }

        private float FixedDistance(Vector2 a, Vector2 b)
        {
            float x = Math.Abs(a.X - b.X);
            float fx = gridSize.X * cellSize - x;
            if (fx < x) x = fx;

            return (float)Math.Sqrt(Math.Pow(x, 2) + Math.Pow(Math.Abs(a.Y - b.Y), 2));
        }

        public void GenerateCircles(ref List<Circle> currentCircles, ref List<Circle> disabledCircles, int layers)
        {
            int generateTo = lastGeneratedIndex + layers;

            //get acrtive from last generated row
            List<Circle> active = new List<Circle>();
            for (int i = 0; i < gridSize.X; i ++)
            {
                Circle c = GetFromGrid(new Point(i, lastGeneratedIndex));
                if (c != null)
                    active.Add(c);
            }
            if (active.Count == 0)
            {
                Circle c = new Circle(new Vector2(0 + 0.5f, lastGeneratedIndex * gridSize.Y + 0.5f));
                active.Add(c);
                SetInGrid(new Point(0, lastGeneratedIndex), c);
                currentCircles.Add(c);
            }

            //clean field
            for (int x = 0; x < gridSize.X; x++)
                for (int y = lastGeneratedIndex + 1; y <= generateTo; y++)
                {
                    Circle c = GetFromGrid(new Point(x, y));
                    if (c != null)
                    {
                        disabledCircles.Add(c);
                        currentCircles.Remove(c);
                    }
                    SetInGrid(new Point(x, y), null);
                }
            int a = 1;
            //generate
            while (active.Count > 0)
            {
                //Debug.WriteLine(currentCircles.Count);
                int randomIndex = random.Next(0, active.Count);

                bool found = false;
                for (int n = 0; n < k; n++)
                {
                    //make random neightbour point
                    double angle = random.NextDouble() * Math.PI * 2;
                    Vector2 offset = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                    offset *= (float)(r + random.NextDouble() * r);

                    Vector2 p = FixedPos(active[randomIndex].Position + offset);
                    Point inGridPos = PosToIndex(p);

                    if (inGridPos.Y <= lastGeneratedIndex || inGridPos.Y > generateTo)
                        continue;

                    bool fit = true;
                    for (int x = -1; x <= 1; x++)
                    {
                        int xIndex = inGridPos.X + x;

                        for (int y = -1; y <= 1; y++)
                        {
                            int yIndex = inGridPos.Y + y;
                            if (yIndex <= lastGeneratedIndex || yIndex > generateTo)
                                continue;

                            Circle c = GetFromGrid(new Point(xIndex, yIndex));
                            if (c != null && FixedDistance(p, c.Position) < r)
                            {
                                fit = false;
                                break;
                            }
                        }
                    }

                    if (fit)
                    {
                        Circle c = GenerateCircle(p, ref disabledCircles, GetEffect());
                        currentCircles.Add(c);
                        active.Add(c);
                        SetInGrid(inGridPos, c);
                        found = true;
                        break;
                    }
                }

                
                if (!found)
                    active.RemoveAt(randomIndex);

            }
            lastGeneratedIndex = generateTo;
        }

        public Circle GenerateCircle(Vector2 position, ref List<Circle> disabledCircles, EffectType effectType = EffectType.None)
        {
            Circle c;

            if (disabledCircles.Count > 0)
            {
                c = disabledCircles[0];
                c.Setup(position, effectType);
                disabledCircles.RemoveAt(0);
            }
            else
            {
                c = new Circle(position, effectType);
            }

            return c;
        }

        private EffectType GetEffect()
        {
            double rand = random.NextDouble();

            if (rand <= freezeChance)
                return EffectType.Freeze;

            rand -= freezeChance;

            return EffectType.None;
        }

        private bool isClamp(Vector2 point, float left, float right, float bot, float top)
        {
            return (point.X > left && point.X < right && point.Y >= bot && point.Y < top);
        }

        private float mod(float x, float m)
        {
            float r = x % m;
            return r < 0 ? r + m : r;
        }
        private int mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
