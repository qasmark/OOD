using SFML.System;
using SFML.Window;
using Shapes.Applications;
using Shapes.Decorators;
using Shapes.Handlers;

namespace Shapes.State
{
    public sealed class DragAndDropState : IState
    {
        private readonly Application _application;
        private CanvasHandler CanvasHandler => _application.CanvasHandler;

        public DragAndDropState(Application application)
        {
            _application = application;
        }

        public void SimpleClick(Vector2i click)
        {
            ShapeDecorator activeShape = _application.CanvasHandler.GetActivatedShape(click.X, click.Y);
            if (Keyboard.IsKeyPressed(Keyboard.Key.LShift))
            {
                CanvasHandler.SelectShape(activeShape);
            }
            else
            {
                if (!CanvasHandler.SelectedShapes.Contains(activeShape))
                {
                    CanvasHandler.ForceSelectShape(activeShape);
                }
            }
        }

        public void PressedKeyG()
        {
            CanvasHandler.SaveToHistory();
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                _application.CanvasHandler.GroupShapes();
            }
        }

        public void PressedKeyU()
        {
            CanvasHandler.SaveToHistory();
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                _application.CanvasHandler.UngroupShapes();
            }
        }

        public void MouseMoved(Vector2i mousePosition)
        {
            ShapeDecorator activeShape = CanvasHandler.GetActivatedShape(mousePosition.X, mousePosition.Y);
            if (Mouse.IsButtonPressed(Mouse.Button.Left) && _application.CanvasHandler.SelectedShapes.Contains(activeShape))
            {
                if (!_application.IsSomethingMoved)
                {
                    CanvasHandler.SaveToHistory();
                }
                int moveX = (mousePosition.X - _application.ClickMousePosition.X);
                int moveY = (mousePosition.Y - _application.ClickMousePosition.Y);
                CanvasHandler.MoveShapes(moveX, moveY);

                _application.SetClickMousePosition(new Vector2i(
                    _application.ClickMousePosition.X + moveX,
                    _application.ClickMousePosition.Y + moveY));

                _application.SetSomethingIsMoved(true);
            }
        }

        public void MouseReleased()
        {
            if (_application.IsSomethingMoved)
            {
                _application.SetSomethingIsMoved(false);
            }
        }
    }
}
