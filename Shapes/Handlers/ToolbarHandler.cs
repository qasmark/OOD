using SFML.Graphics;
using SFML.System;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Iterators;
using Shapes.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Handlers
{
    public sealed class ToolbarHandler : BaseShapeHandler
    {
        private readonly Action _setDragAndDropStateAction;
        private readonly Action _setAddShapeStateAction;
        private readonly Action _setChangeOutlineColorStateAction;
        private readonly Action _setChangeFillColorStateAction;
        private readonly Action _setChangeOutlineThicknessStateAction;
        private readonly Action _undoAction;

        private readonly DefaultsListIterator<Color> _outlineColors
            = new(new List<Color>()
            {
                DefaultColors.Blue,
                DefaultColors.Red
            });
        private readonly DefaultsListIterator<Color> _fillColors
            = new(new List<Color>()
            {
                DefaultColors.Green,
                DefaultColors.Orange,
                DefaultColors.Violet
            });
        private readonly DefaultsListIterator<int> _outlineThicknesses
            = new(new List<int>()
            {
                1,
                2,
                3
            });
        private readonly DefaultsListIterator<int> _addShapeType
            = new(new List<int>()
            {
                (int)ShapeType.Triangle,
                (int)ShapeType.Rectangle,
                (int)ShapeType.Circle
            });

        private const int Height = 50;

        public Color CurrentFillColor => _fillColors.GetCurrent();
        public Color CurrentOutlineColor => _outlineColors.GetCurrent();
        public int CurrentOutlineThickness => _outlineThicknesses.GetCurrent();
        public ShapeType CurrentAddShapeType => (ShapeType)_addShapeType.GetCurrent();

        private ToolbarHandler(
            ShapeDecoratorGroup shapesGroup,
            SelectedShapeDecoratorGroup selectedShapesGroup,
            Action setDragAndDropStateAction,
            Action setAddShapeStateAction,
            Action setChangeFillColorStateAction,
            Action setChangeOutlineColorStateAction,
            Action setChangeOutlineThicknessStateAction,
            Action undoAction)
            : base(shapesGroup, selectedShapesGroup)
        {
            _setDragAndDropStateAction = setDragAndDropStateAction;
            _setAddShapeStateAction = setAddShapeStateAction;
            _setChangeFillColorStateAction = setChangeFillColorStateAction;
            _setChangeOutlineColorStateAction = setChangeOutlineColorStateAction;
            _setChangeOutlineThicknessStateAction = setChangeOutlineThicknessStateAction;
            _undoAction = undoAction;

            foreach (ToolbarButtonType type in Enum.GetValues(typeof(ToolbarButtonType)))
            {
                ButtonRectangleDecorator newButton = new(type, GetButtonCommand(type), GetButtonText(type));

                ButtonRectangleDecorator lastButton = (ButtonRectangleDecorator)_shapesGroup.Shapes.LastOrDefault();
                if (lastButton != null)
                {
                    int moveX = (int)(lastButton.Position.X + lastButton.Size.X + 20);
                    newButton.Move(moveX, 0);
                }
                AddShape(newButton);
            }

            ButtonRectangleDecorator firstButton = (ButtonRectangleDecorator)_shapesGroup.Shapes.FirstOrDefault();
            if (firstButton != null)
            {
                SelectButton(firstButton);
                firstButton.Press();
            }
        }

        public ToolbarHandler(
            Action setDragAndDropStateAction,
            Action setAddShapeStateAction,
            Action setChangeFillColorStateAction,
            Action setChangeOutlineColorStateAction,
            Action setChangeOutlineThicknessStateAction,
            Action undoAction)
            : this(
                ShapeDecoratorFactory.GetShapeDecoratorGroup(),
                ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup(),
                setDragAndDropStateAction,
                setAddShapeStateAction,
                setChangeFillColorStateAction,
                setChangeOutlineColorStateAction,
                setChangeOutlineThicknessStateAction,
                undoAction)
        { }

        public bool HandleClick(int mouseX, int mouseY)
        {
            if (mouseY > Height)
            {
                return false;
            }

            ButtonRectangleDecorator activeButton = (ButtonRectangleDecorator)GetActivatedShape(mouseX, mouseY);
            if (activeButton != null)
            {
                if (activeButton.Type == ToolbarButtonType.Undo)
                {
                    activeButton.Press();
                    return true;
                }

                if (_selectedShapesGroup.Shapes.Contains(activeButton))
                {
                    ChangeToolbarParams(activeButton.Type);
                }
                else
                {
                    SelectButton(activeButton);
                    activeButton.Press();
                }
            }

            return true;
        }

        public override void Draw(RenderWindow window)
        {
            RectangleDecorator background = new(new RectangleShape());
            background.SetPosition(new Vector2f(0, 0));
            background.SetSize(new Vector2f(window.Size.X, Height));
            background.SetFillColor(DefaultColors.Dark);
            background.Draw(window);

            RectangleDecorator currentColors = new(new RectangleShape());
            currentColors.SetPosition(new Vector2f(window.Size.X - 50, 9));
            currentColors.SetSize(new Vector2f(32, 32));
            currentColors.SetFillColor(CurrentFillColor);
            currentColors.SetOutlineColor(CurrentOutlineColor);
            currentColors.SetOutlineThickness(4);
            currentColors.Draw(window);

            Text currentOutlineThickness = new(CurrentOutlineThickness.ToString(), DefaultFiles.Font, 20);
            currentOutlineThickness.Position = new Vector2f(window.Size.X - 39, 13);
            window.Draw(currentOutlineThickness);

            Text currentShapeType = new(
                CurrentAddShapeType.ToString()[0].ToString(),
                DefaultFiles.Font, 20);
            currentShapeType.Position = new Vector2f(window.Size.X - 80, 13);
            window.Draw(currentShapeType);

            _shapesGroup.Draw(window);
            _selectedShapesGroup.Draw(window);
        }

        private void SelectButton(ButtonRectangleDecorator button)
        {
            UnselectAll();
            Select(button);
        }

        private void ChangeToolbarParams(ToolbarButtonType type)
        {
            switch (type)
            {
                case ToolbarButtonType.AddShape:
                    _addShapeType.MoveNext();
                    break;
                case ToolbarButtonType.ChangeOutlineColor:
                    _outlineColors.MoveNext();
                    break;
                case ToolbarButtonType.ChangeFillColor:
                    _fillColors.MoveNext();
                    break;
                case ToolbarButtonType.ChangeOutlineThickness:
                    _outlineThicknesses.MoveNext();
                    break;
                default:
                    return;
            }
        }

        private Action GetButtonCommand(ToolbarButtonType type)
        {
            return type switch
            {
                ToolbarButtonType.DragAndDrop => _setDragAndDropStateAction,
                ToolbarButtonType.AddShape => _setAddShapeStateAction,
                ToolbarButtonType.ChangeOutlineColor => _setChangeOutlineColorStateAction,
                ToolbarButtonType.ChangeFillColor => _setChangeFillColorStateAction,
                ToolbarButtonType.ChangeOutlineThickness => _setChangeOutlineThicknessStateAction,
                ToolbarButtonType.Undo => _undoAction,
                _ => throw new ArgumentOutOfRangeException($"Unknown toolbar button type {type}")
            };
        }

        private static string GetButtonText(ToolbarButtonType type)
        {
            return type switch
            {
                ToolbarButtonType.DragAndDrop => "D&D",
                ToolbarButtonType.AddShape => "Add",
                ToolbarButtonType.ChangeOutlineColor => "Outline Color",
                ToolbarButtonType.ChangeFillColor => "Fill Color",
                ToolbarButtonType.ChangeOutlineThickness => "Outline Thickness",
                ToolbarButtonType.Undo => "Undo",
                _ => throw new ArgumentOutOfRangeException($"Unknown toolbar button type {type}")
            };
        }
    }
}
