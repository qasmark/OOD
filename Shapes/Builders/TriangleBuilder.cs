using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using System;
using System.Collections.Generic;

namespace Shapes.Builders
{
    public sealed class TriangleBuilder : ShapeBuilder
    {
        private readonly List<int> _parameters;
        private readonly List<uint> _colors;
        private readonly TriangleDecorator _triangle;

        public TriangleBuilder(List<string> parameters)
        {
            if (parameters.Count != 11)
            {
                throw new ArgumentException("Invalid count of triangle params");
            }

            _parameters = new List<int>();
            _colors = new List<uint>();

            parameters.ForEach(x =>
            {
                if (int.TryParse(x, out int parameter))
                {
                    _parameters.Add(parameter);
                }
                else
                {
                    _colors.Add(uint.Parse(x));
                }
            });

            if (_colors.Count != 2)
            {
                throw new ArgumentException("Invalid count of triangle params");
            }

            _triangle = new TriangleDecorator(new ConvexShape());
        }

        public override void Build()
        {
            _triangle.SetPointCount(3);
            _triangle.SetPoint(0, new Vector2f(_parameters[0], _parameters[1]));
            _triangle.SetPoint(1, new Vector2f(_parameters[2], _parameters[3]));
            _triangle.SetPoint(2, new Vector2f(_parameters[4], _parameters[5]));
            _triangle.SetPosition(
                new Vector2f(
                    _parameters[6],
                    _parameters[7]));
            _triangle.SetFillColor(new Color(_colors[0]));
            _triangle.SetOutlineColor(new Color(_colors[1]));
            _triangle.SetOutlineThickness(_parameters[8]);
        }

        public override TriangleDecorator GetResult()
        {
            return _triangle;
        }
    }
}
