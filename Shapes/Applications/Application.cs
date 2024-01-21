using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Handlers;
using Shapes.State;
using Shapes.Types;
using Shapes.Visitors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shapes.Applications
{
    public sealed class Application
    {
        private readonly RenderWindow _window;
        private readonly StreamReader _input;
        private readonly StreamWriter _output;
        private readonly ToolbarHandler _toolbarHandler;
        private readonly CanvasHandler _canvasHandler;
        private readonly SaveCanvasHandler _saveCanvasHandler;

        private readonly DragAndDropState _dragAndDropState;
        private readonly AddShapeState _addShapeState;
        private readonly ChangeFillColorState _changeFillColorState;
        private readonly ChangeOutlineColorState _changeOutlineColorState;
        private readonly ChangeOutlineThicknessState _changeOutlineThicknessState;
        private Vector2i _clickMousePosition;
        private IState _state;
        private bool _somethingIsMoved;

        public CanvasHandler CanvasHandler => _canvasHandler;
        public ToolbarHandler ToolbarHandler => _toolbarHandler;
        public Vector2i ClickMousePosition => _clickMousePosition;
        public bool IsSomethingMoved => _somethingIsMoved;

        public Application(RenderWindow window, StreamReader input, StreamWriter output)
        {

            _input = input;
            _output = output;

            _dragAndDropState = new DragAndDropState(this);
            _addShapeState = new AddShapeState(this);
            _changeFillColorState = new ChangeFillColorState(this);
            _changeOutlineColorState = new ChangeOutlineColorState(this);
            _changeOutlineThicknessState = new ChangeOutlineThicknessState(this);

            _toolbarHandler = new ToolbarHandler(
                SetDragAndDropStateAction,
                SetAddShapeStateAction,
                SetChangeFillColorStateAction,
                SetChangeOutlineColorStateAction,
                SetChangeOutlineThicknessStateAction,
                UndoAction);
            _canvasHandler = new CanvasHandler();
            _saveCanvasHandler = new SaveCanvasHandler(_canvasHandler);

            _window = window;
            _window.Closed += WindowClosed;
            _window.MouseButtonPressed += WindowMousePressed;
            _window.MouseButtonReleased += WindowMouseReleased;
            _window.KeyPressed += WindowKeyPressed;
            _window.MouseMoved += WindowMouseMoved;

            _clickMousePosition = new Vector2i();
            _somethingIsMoved = false;
        }

        public void ReadDefaultShapes()
        {
            while (!_input.EndOfStream)
            {
                List<string> shapeParams = _input
                    .ReadLine()
                    .Split(" ")
                    .Where(s => !string.IsNullOrEmpty(s))
                    .ToList();
                if (shapeParams.Count == 0 || !Enum.TryParse(shapeParams[0], true, out ShapeType shapeType))
                {
                    continue;
                }

                shapeParams.RemoveAt(0);
                ShapeDecorator shape = shapeType switch
                {
                    ShapeType.Triangle => ShapeDecoratorFactory.GetTriangleDecorator(shapeParams),
                    ShapeType.Rectangle => ShapeDecoratorFactory.GetRectangleDecorator(shapeParams),
                    ShapeType.Circle => ShapeDecoratorFactory.GetCircleDecorator(shapeParams),
                    _ => throw new ArgumentOutOfRangeException($"Unknown shape type: {shapeType}")
                };
                CanvasHandler.AddShape(shape);
            }
        }

        public void PrintShapesInfo()
        {
            ShapeDescriptionVisitor descriptionVisitor = new(_output);
            List<ShapeDecorator> shapes = CanvasHandler.Shapes
                .Concat(CanvasHandler.SelectedShapes)
                .ToList();
            foreach (ShapeDecorator shape in shapes)
            {
                shape.Accept(descriptionVisitor);
            }
        }

        public void ProcessWindow()
        {
            while (_window.IsOpen)
            {
                _window.DispatchEvents();
                _window.Clear(DefaultColors.BlueDark);

                DrawApplication();

                _window.Display();
            }
        }

        private void DrawApplication()
        {
            CanvasHandler.Draw(_window);
            ToolbarHandler.Draw(_window);
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            _window.Close();
        }

        private void WindowMousePressed(object sender, MouseButtonEventArgs e)
        {
            switch (e.Button)
            {
                case Mouse.Button.Left:
                    int mouseX = e.X;
                    int mouseY = e.Y;
                    _clickMousePosition.X = mouseX;
                    _clickMousePosition.Y = mouseY;

                    if (ToolbarHandler.HandleClick(mouseX, mouseY))
                    {
                        return;
                    }

                    _state.SimpleClick(new Vector2i(e.X, e.Y));
                    break;
                default:
                    return;
            }
        }

        private void WindowMouseReleased(object sender, MouseButtonEventArgs e)
        {
            _state.MouseReleased();
        }

        private void WindowKeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.G:
                    _state.PressedKeyG();
                    break;
                case Keyboard.Key.U:
                    _state.PressedKeyU();
                    break;
                case Keyboard.Key.T:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                        {
                            _saveCanvasHandler.SetSaveTextStrategy();
                            _saveCanvasHandler.Save(DefaultFiles.SaveFileName);
                        }
                        if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                        {
                            _saveCanvasHandler.SetOpenTextStrategy();
                            _saveCanvasHandler.Open(DefaultFiles.OpenFileName);
                        }
                    }
                    break;
                case Keyboard.Key.B:
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    {
                        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                        {
                            _saveCanvasHandler.SetSaveBinaryStrategy();
                            _saveCanvasHandler.Save(DefaultFiles.SaveFileName);
                        }
                        if (Keyboard.IsKeyPressed(Keyboard.Key.O))
                        {
                            _saveCanvasHandler.SetOpenBinaryStrategy();
                            _saveCanvasHandler.Open(DefaultFiles.OpenFileName);
                        }
                    }
                    break;
                default:
                    return;
            }
        }

        private void WindowMouseMoved(object sender, MouseMoveEventArgs e)
        {
            _state.MouseMoved(new Vector2i(e.X, e.Y));
        }

        private void SetDragAndDropStateAction() => _state = _dragAndDropState;
        private void SetAddShapeStateAction()
        {
            _state = _addShapeState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeOutlineColorStateAction()
        {
            _state = _changeOutlineColorState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeFillColorStateAction()
        {
            _state = _changeFillColorState;
            CanvasHandler.UnselectAll();
        }
        private void SetChangeOutlineThicknessStateAction()
        {
            _state = _changeOutlineThicknessState;
            CanvasHandler.UnselectAll();
        }

        private void UndoAction()
        {
            CanvasHandler.RollBackHistory();
        }

        public void SetClickMousePosition(Vector2i position) => _clickMousePosition = position;
        public void SetSomethingIsMoved(bool isSomethingMoved) => _somethingIsMoved = isSomethingMoved;
    }
}