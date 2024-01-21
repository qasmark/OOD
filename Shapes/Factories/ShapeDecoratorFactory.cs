using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Factories
{
    public static class ShapeDecoratorFactory
    {
        public static TriangleDecorator GetTriangleDecorator(List<string> parameters)
        {
            if (parameters.Count != 6)
            {
                throw new ArgumentException("Invalid count of triangle parameters");
            }

            return GetTriangle(GetCoordinates(parameters));
        }

        public static TriangleDecorator GetTriangleDecorator()
        {
            return GetTriangle(new List<Tuple<int, int>>()
            {
                Tuple.Create(0, 0),
                Tuple.Create(100, 100),
                Tuple.Create(200, 0)
            });
        }

        public static RectangleDecorator GetRectangleDecorator(List<string> parameters)
        {
            if (parameters.Count != 4)
            {
                throw new ArgumentException("Invalid count of rectangle parameters");
            }

            return GetRectangle(GetCoordinates(parameters));
        }

        public static RectangleDecorator GetRectangleDecorator()
        {
            return GetRectangle(new List<Tuple<int, int>>()
            {
                Tuple.Create(0, 0),
                Tuple.Create(100, 100)
            });
        }

        public static CircleDecorator GetCircleDecorator(List<string> parameters)
        {
            if (parameters.Count != 3)
            {
                throw new ArgumentException("Invalid count of circle parameters");
            }

            int radius = int.Parse(parameters[2]);

            return GetCircle(GetCoordinates(parameters).First(), radius);
        }

        public static CircleDecorator GetCircleDecorator()
        {
            return GetCircle(Tuple.Create(0, 0), 50);
        }

        public static ShapeDecoratorGroup GetShapeDecoratorGroup()
        {
            return new ShapeDecoratorGroup();
        }

        public static SelectedShapeDecoratorGroup GetSelectedShapeDecoratorGroup()
        {
            return new SelectedShapeDecoratorGroup();
        }

        private static TriangleDecorator GetTriangle(List<Tuple<int, int>> coordinates)
        {
            TriangleDecorator triangle = new(new ConvexShape());
            triangle.SetPosition(new Vector2f(50, 50));
            foreach (Tuple<int, int> pair in coordinates)
            {
                uint pointCount = triangle.GetPointCount();
                triangle.SetPointCount(pointCount + 1);
                triangle.SetPoint(pointCount, new Vector2f(pair.Item1, pair.Item2));
            }

            triangle.SetFillColor(DefaultColors.Violet);

            return triangle;
        }

        private static RectangleDecorator GetRectangle(List<Tuple<int, int>> coordinates)
        {
            RectangleDecorator rectangle = new(new RectangleShape());
            rectangle.SetPosition(new Vector2f(
                (coordinates[0].Item1 + coordinates[1].Item1) / 2,
                (coordinates[0].Item2 + coordinates[1].Item2) / 2));
            rectangle.SetSize(new Vector2f(
                coordinates[1].Item1 - coordinates[0].Item1,
                coordinates[1].Item2 - coordinates[0].Item2));
            rectangle.SetFillColor(DefaultColors.Orange);

            return rectangle;
        }

        private static CircleDecorator GetCircle(Tuple<int, int> coordinates, int radius)
        {
            CircleDecorator circle = new(new CircleShape());
            circle.SetRadius(radius);

            circle.SetPosition(new Vector2f(coordinates.Item1, coordinates.Item2));
            circle.SetFillColor(DefaultColors.Green);

            return circle;
        }

        private static List<Tuple<int, int>> GetCoordinates(List<string> parameters)
        {
            if (parameters.Count % 2 != 0)
            {
                parameters.Remove(parameters.Last());
            }

            return parameters
                .Where((x, i) => i % 2 == 0)
                .Zip(
                    parameters.Where((x, i) => i % 2 != 0),
                    (a, b) => Tuple.Create(
                        int.Parse(a),
                        int.Parse(b)))
                .ToList();
        }
    }
}
