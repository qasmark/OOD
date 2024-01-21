using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;
using Shapes.Visitors;
using System;
using System.Collections.Generic;

namespace Shapes.Compounds
{
    public class ShapeDecoratorGroup : ShapeDecorator
    {
        private readonly List<ShapeDecorator> _shapes;

        public List<ShapeDecorator> Shapes => _shapes;

        public ShapeDecoratorGroup()
            : base() 
        {
            _shapes = new();
        }

        public void Add(ShapeDecorator shape)
        {
            if (_shapes.Contains(shape))
            {
                return;
            }

            _shapes.Add(shape);
        }

        public void Remove(ShapeDecorator shape)
        {
            _shapes.Remove(shape);
        }

        public void RemoveAll()
        {
            _shapes.Clear();
        }

        public override int GetArea()
        {
            throw new NotImplementedException();
        }

        public override int GetPerimeter()
        {
            throw new NotImplementedException();
        }

        public override void Accept(ShapeVisitor visitor)
        {
            _shapes.ForEach(x => x.Accept(visitor));
        }

        public override void Move(int moveX, int moveY)
        {
            _shapes.ForEach(x => x.Move(moveX, moveY));
        }

        public override void SetFillColor(Color color)
        {
            _shapes.ForEach(x => x.SetFillColor(color));
        }

        public override void SetOutlineColor(Color color)
        {
            _shapes.ForEach(x => x.SetOutlineColor(color));
        }

        public override void SetOutlineThickness(int thickness)
        {
            _shapes.ForEach(x => x.SetOutlineThickness(thickness));
        }

        public override void Draw(RenderWindow window)
        {
            _shapes.ForEach(x => x.Draw(window));
        }

        public override IntRect GetGlobalBounds()
        {
            IntRect frame = _shapes[0].GetGlobalBounds();
            Vector2i leftTopPoint;
            leftTopPoint.X = frame.Left;
            leftTopPoint.Y = frame.Top;

            Vector2i rightBottomPoint;
            rightBottomPoint.X = frame.Left + frame.Width;
            rightBottomPoint.Y = frame.Top + frame.Height;

            foreach (ShapeDecorator shape in _shapes)
            {
                IntRect currentFrame = shape.GetGlobalBounds();

                if (currentFrame.Left < leftTopPoint.X)
                {
                    leftTopPoint.X = currentFrame.Left;
                }
                if (currentFrame.Top < leftTopPoint.Y)
                {
                    leftTopPoint.Y = currentFrame.Top;
                }
                if (currentFrame.Left + currentFrame.Width > rightBottomPoint.X)
                {
                    rightBottomPoint.X = currentFrame.Left + currentFrame.Width;
                }
                if (currentFrame.Top + currentFrame.Height > rightBottomPoint.Y)
                {
                    rightBottomPoint.Y = currentFrame.Top + currentFrame.Height;
                }
            }

            return new IntRect(
                leftTopPoint.X, 
                leftTopPoint.Y, 
                rightBottomPoint.X - leftTopPoint.X, 
                rightBottomPoint.Y - leftTopPoint.Y);
        }

        public override ShapeDecoratorGroup Clone()
        {
            ShapeDecoratorGroup newGroup = new();
            Shapes.ForEach(x => newGroup.Add(x.Clone()));

            return newGroup;
        }
    }
}
