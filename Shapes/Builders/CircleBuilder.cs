using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using System;
using System.Collections.Generic;

namespace Shapes.Builders
{
    public sealed class CircleBuilder : ShapeBuilder
    {
        private readonly List<int> _parameters;
        private readonly List<uint> _colors;
        private readonly CircleDecorator _circle;

        public CircleBuilder(List<string> parameters)
        {
            if (parameters.Count != 6)
            {
                throw new ArgumentException("Invalid count of circle params");
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
                throw new ArgumentException("Invalid count of circle params");
            }

            _circle = new CircleDecorator(new CircleShape());
        }

        public override void Build()
        {
            _circle.SetRadius(_parameters[0]);
            _circle.SetPosition(
                new Vector2f(
                    _parameters[1],
                    _parameters[2]));
            _circle.SetFillColor(new Color(_colors[0]));
            _circle.SetOutlineColor(new Color(_colors[1]));
            _circle.SetOutlineThickness(_parameters[3]);
        }

        public override CircleDecorator GetResult()
        {
            return _circle;
        }
    }
}
