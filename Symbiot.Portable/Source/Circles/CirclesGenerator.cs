using Microsoft.Xna.Framework;
using Symbiot.Portable.Source.Controllers;
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
        private readonly Vector2 field = new Vector2(25, 25);
        private const int k = 10;
        private float r = 3.6f;


        //for generate cycle
        private float botBorder;
        private float leftBorder;
        private float rightBorder;
        private float topBorder;

        private List<Circle> activeCircles = new List<Circle>();
        private Random random;

        public bool Finished => (activeCircles.Count == 0);

        public void BeginGenerating(ref List<Circle> currentCircles)
        {
            botBorder = CameraController.Instance.Position.Y;
            leftBorder = CameraController.Instance.Position.X - field.X / 2;
            rightBorder = leftBorder + field.X;
            topBorder = botBorder + field.Y;

            random = new Random();
            activeCircles = new List<Circle>();

            foreach (Circle c in currentCircles)
                if (isClamp(c.Position, leftBorder, rightBorder, botBorder, topBorder))
                {
                    activeCircles.Add(c);
                }
        }

        public Circle GenerateCircle(ref List<Circle> currentCircles, ref List<Circle> disabledCircles)
        {
            //Debug.WriteLine(activeCircles.Count);
            int index = random.Next(0, activeCircles.Count);

            Vector2 newCPos = new Vector2(-1);
            bool found = false;
            for (int i = 0; i < k; i++)
            {
                float rd = (float)(random.NextDouble() * 0.8 * Math.PI + 0.1 * Math.PI);
                newCPos = activeCircles[index].Position + new Vector2((float)Math.Cos(rd), (float)Math.Sin(rd)) * (r + (float)random.NextDouble() * r);


                if (!isClamp(newCPos, leftBorder, rightBorder, botBorder, topBorder))
                    continue;

                bool fit = true;
                for (int j = 0; j < activeCircles.Count; j++)
                    if (Vector2.Distance(activeCircles[j].Position, newCPos) < r)
                    {
                        fit = false;
                        break;
                    }
                if (fit)
                    for (int j = 0; j < currentCircles.Count; j++)
                        if (Vector2.Distance(currentCircles[j].Position, newCPos) < r)
                        {
                            fit = false;
                            break;
                        }

                //adding new circle
                if (fit)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                Circle c = GenerateCircle(newCPos, ref disabledCircles);

                activeCircles.Add(c);
                return c;
            }
            else
            {
                if (!currentCircles.Contains(activeCircles[index]))
                    currentCircles.Add(activeCircles[index]);
                activeCircles.RemoveAt(index);
            }

            return null;
        }

        public Circle GenerateCircle(Vector2 position, ref List<Circle> disabledCircles)
        {
            Circle c;

            if (disabledCircles.Count > 0)
            {
                c = disabledCircles[0];
                c.Setup(position);
                disabledCircles.RemoveAt(0);
            }
            else
            {
                c = new Circle(position);
            }

            return c;
        }

        bool isClamp(Vector2 point, float left, float right, float bot, float top)
        {
            return (point.X > left && point.X < right && point.Y >= bot && point.Y < top);
        }
    }
}
