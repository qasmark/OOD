using SFML.Graphics;
using Shapes.Visitors;
using System;

namespace Shapes.Decorators
{
    public sealed class CircleDecorator : ShapeDecorator
    {
        private CircleShape Circle => (CircleShape)_shape;

        public int Radius => (int)Circle.Radius;

        public CircleDecorator(CircleShape shape)
            : base(shape) { }

        public void SetRadius(int radius)
        {
            Circle.Radius = radius;
        }

        public override int GetArea()
        {
            return (int)(Circle.Radius * Circle.Radius * Math.PI);
        }

        public override int GetPerimeter()
        {
            return 2 * (int)(Circle.Radius * Math.PI);
        }

        public override void Accept(ShapeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override CircleDecorator Clone()
        {
            CircleDecorator newCircle = new(new CircleShape());
            newCircle.SetRadius((int)Circle.Radius);
            newCircle.SetPosition(Circle.Position);
            newCircle.SetFillColor(Circle.FillColor);
            newCircle.SetOutlineColor(Circle.OutlineColor);
            newCircle.SetOutlineThickness((int)Circle.OutlineThickness);

            return newCircle;
        }
    }
}
