using Shapes.Compounds;
using Shapes.Decorators;
using System.IO;

namespace Shapes.Visitors
{
    public abstract class ShapeVisitor
    {
        protected static StreamWriter _outputFileStream;

        public ShapeVisitor(StreamWriter outputStream)
        {
            _outputFileStream = outputStream;
        }

        public abstract void Visit(TriangleDecorator triangleDecorator);
        public abstract void Visit(RectangleDecorator rectangleDecorator);
        public abstract void Visit(CircleDecorator circleDecorator);
    }
}
