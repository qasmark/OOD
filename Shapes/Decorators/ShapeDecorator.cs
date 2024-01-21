using SFML.Graphics;
using SFML.System;
using Shapes.Visitors;
using System;

namespace Shapes.Decorators
{
    public abstract class ShapeDecorator
    {
        protected readonly Shape _shape;

        public Vector2f Position => _shape.Position;
        public Color FillColor => _shape.FillColor;
        public Color OutlineColor => _shape.OutlineColor;
        public int OutlineThickness => (int)_shape.OutlineThickness;

        public ShapeDecorator(Shape shape)
        {
            _shape = shape;
        }

        protected ShapeDecorator() { }

        public abstract int GetArea();
        public abstract int GetPerimeter();
        public abstract void Accept(ShapeVisitor visitor);
        
        public uint GetPointCount()
        {
            return _shape.GetPointCount();
        }

        public Vector2f GetPoint(uint index)
        {
            return _shape.GetPoint(index);
        }

        public virtual void SetPosition(Vector2f position)
        {
            _shape.Position = position;
        }

        public virtual void SetFillColor(Color color)
        {
            _shape.FillColor = color;
        }

        public virtual void SetOutlineThickness(int thickness)
        {
            _shape.OutlineThickness = thickness;
        }

        public virtual void SetOutlineColor(Color color)
        {
            _shape.OutlineColor = color;
        }

        public virtual void Move(int moveX, int moveY)
        {
            Vector2f currentPosition = _shape.Position;
            _shape.Position = new Vector2f(currentPosition.X + moveX, currentPosition.Y + moveY);
        }

        public virtual void Draw(RenderWindow window)
        {
            window.Draw(_shape);
        }

        public virtual IntRect GetGlobalBounds()
        {
            return (IntRect)_shape.GetGlobalBounds();
        }

        public abstract ShapeDecorator Clone();
    }
}
