using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using System;
using System.Collections.Generic;

namespace Shapes.Builders
{
    public sealed class RectangleBuilder : ShapeBuilder
    {
        private readonly List<int> _parameters;
        private readonly List<uint> _colors;
        private readonly RectangleDecorator _rectangle;

        public RectangleBuilder(List<string> parameters)
        {
            if (parameters.Count != 7)
            {
                throw new ArgumentException("Invalid count of rectangle params");
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
                throw new ArgumentException("Invalid count of rectangle params");
            }

            _rectangle = new RectangleDecorator(new RectangleShape());
        }

        public override void Build()
        {
            _rectangle.SetPosition(
                new Vector2f(
                    _parameters[0],
                    _parameters[1]));
            _rectangle.SetSize(
                new Vector2f(
                    _parameters[2],
                    _parameters[3]));
            _rectangle.SetFillColor(new Color(_colors[0]));
            _rectangle.SetOutlineColor(new Color(_colors[1]));
            _rectangle.SetOutlineThickness(_parameters[4]);
        }

        public override RectangleDecorator GetResult()
        {
            return _rectangle;
        }
    }
}
