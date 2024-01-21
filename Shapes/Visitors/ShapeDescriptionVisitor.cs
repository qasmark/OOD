using Shapes.Compounds;
using Shapes.Decorators;
using System;
using System.IO;

namespace Shapes.Visitors
{
    class ShapeDescriptionVisitor : ShapeVisitor
    {
        private const string TriangleShapeType = "TRIANGLE";
        private const string RectangleShapeType = "RECTANGLE";
        private const string CircleShapeType = "CIRCLE";

        public ShapeDescriptionVisitor(StreamWriter outputStream) 
            : base(outputStream) { }

        public override void Visit(TriangleDecorator triangleDecorator)
        {
            DisplayShapeDescription(TriangleShapeType, triangleDecorator.GetArea(), triangleDecorator.GetPerimeter());
        }

        public override void Visit(RectangleDecorator rectangleDecorator)
        {
            DisplayShapeDescription(RectangleShapeType, rectangleDecorator.GetArea(), rectangleDecorator.GetPerimeter());
        }

        public override void Visit(CircleDecorator circleDecorator)
        {
            DisplayShapeDescription(CircleShapeType, circleDecorator.GetArea(), circleDecorator.GetPerimeter());
        }

        private static void DisplayShapeDescription(string type, int area, int perimeter)
        {
            _outputFileStream.WriteLine($"{type}: P={perimeter}; S={area}");
        }

    }
}
