using SFML.System;
using Shapes.Applications;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.Types;
using System;

namespace Shapes.State
{
    public sealed class AddShapeState : IState
    {
        private readonly Application _application;
        private ToolbarHandler ToolbarHandler => _application.ToolbarHandler;
        private CanvasHandler CanvasHandler => _application.CanvasHandler;

        public AddShapeState(Application application)
        {
            _application = application;
        }

        public void SimpleClick(Vector2i click)
        {
            CanvasHandler.SaveToHistory();
            ShapeType shapeType = ToolbarHandler.CurrentAddShapeType;
            ShapeDecorator shape = shapeType switch
            {
                ShapeType.Triangle => ShapeDecoratorFactory.GetTriangleDecorator(),
                ShapeType.Rectangle => ShapeDecoratorFactory.GetRectangleDecorator(),
                ShapeType.Circle => ShapeDecoratorFactory.GetCircleDecorator(),
                _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}")
            };
            shape.Move((int)(click.X - shape.Position.X), (int)(click.Y - shape.Position.Y));

            CanvasHandler.AddShape(shape);
        }

        public void PressedKeyG() { }

        public void PressedKeyU() { }

        public void MouseMoved(Vector2i mousePosition) { }

        public void MouseReleased() { }
    }
}
