using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Strategies
{
    public abstract class SaveShapesStrategy
    {
        private static string GetTriangleInfo(TriangleDecorator triangle)
        {
            Vector2f firstPoint = triangle.GetPoint(0);
            Vector2f secondPoint = triangle.GetPoint(1);
            Vector2f thirdPoint = triangle.GetPoint(2);
            Vector2f position = triangle.Position;
            Color fillColor = triangle.FillColor;
            Color outlineColor = triangle.OutlineColor;
            int outlineThickness = triangle.OutlineThickness;

            return ShapeType.Triangle.ToString().ToUpper() + " "
                + firstPoint.X + " " + firstPoint.Y + " "
                + secondPoint.X + " " + secondPoint.Y + " "
                + thirdPoint.X + " " + thirdPoint.Y + " "
                + position.X + " " + position.Y + " "
                + fillColor.ToInteger() + " " + outlineColor.ToInteger() + " "
                + outlineThickness;
        }

        private static string GetRectangleInfo(RectangleDecorator rectangle)
        {
            Vector2f position = rectangle.Position;
            Vector2f size = rectangle.Size;
            Color fillColor = rectangle.FillColor;
            Color outlineColor = rectangle.OutlineColor;
            int outlineThickness = rectangle.OutlineThickness;

            return ShapeType.Rectangle.ToString().ToUpper() + " "
                + position.X + " " + position.Y + " "
                + size.X + " " + size.Y + " "
                + fillColor.ToInteger() + " " + outlineColor.ToInteger() + " "
                + outlineThickness;
        }

        private static string GetCircleInfo(CircleDecorator circle)
        {
            int radius = circle.Radius;
            Vector2f position = circle.Position;
            Color fillColor = circle.FillColor;
            Color outlineColor = circle.OutlineColor;
            int outlineThickness = circle.OutlineThickness;

            return ShapeType.Circle.ToString().ToUpper() + " "
                + radius + " "
                + position.X + " " + position.Y + " "
                + fillColor.ToInteger() + " " + outlineColor.ToInteger() + " "
                + outlineThickness;
        }

        protected List<string> GetShapesInfo(List<ShapeDecorator> shapes)
        {
            List<string> shapesInfo = new();
            foreach (ShapeDecorator shape in shapes)
            {
                Type shapeType = shape.GetType();
                if (shapeType == typeof(TriangleDecorator))
                {
                    shapesInfo.Add(GetTriangleInfo((TriangleDecorator)shape));
                }
                else if (shapeType == typeof(RectangleDecorator))
                {
                    shapesInfo.Add(GetRectangleInfo((RectangleDecorator)shape));
                }
                else if (shapeType == typeof(CircleDecorator))
                {
                    shapesInfo.Add(GetCircleInfo((CircleDecorator)shape));
                }
                else if (shapeType == typeof(ShapeDecoratorGroup))
                {
                    shapesInfo = shapesInfo
                        .Concat(GetShapesInfo(((ShapeDecoratorGroup)shape).Shapes))
                        .ToList();
                }
            }

            return shapesInfo;
        }

        public abstract void Save(string fileName, List<ShapeDecorator> shapes);
    }
}
