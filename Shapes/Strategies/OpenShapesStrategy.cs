using Shapes.Builders;
using Shapes.Decorators;
using Shapes.Types;
using System;
using System.Collections.Generic;

namespace Shapes.Strategies
{
    public abstract class OpenShapesStrategy
    {
        protected abstract List<KeyValuePair<ShapeType, List<string>>> GetShapesInfo(string fileName);

        public List<ShapeDecorator> Open(string fileName)
        {
            List<ShapeDecorator> shapes = new();

            List<KeyValuePair<ShapeType, List<string>>> shapesInfo = GetShapesInfo(fileName);

            foreach (KeyValuePair<ShapeType, List<string>> shapeInfo in shapesInfo)
            {
                ShapeBuilder builder = shapeInfo.Key switch
                {
                    ShapeType.Triangle => new TriangleBuilder(shapeInfo.Value),
                    ShapeType.Rectangle => new RectangleBuilder(shapeInfo.Value),
                    ShapeType.Circle => new CircleBuilder(shapeInfo.Value),
                    _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeInfo.Key}"),
                };
                builder.Build();

                shapes.Add(builder.GetResult());
            }

            return shapes;
        }
    }
}
