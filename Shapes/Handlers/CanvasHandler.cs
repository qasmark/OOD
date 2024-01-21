using SFML.Graphics;
using Shapes.Compounds;
using Shapes.Decorators;
using Shapes.Factories;
using Shapes.Mementos;
using System.Collections.Generic;
using System.Linq;

namespace Shapes.Handlers
{
    public sealed class CanvasHandler : BaseShapeHandler
    {
        private readonly List<ShapesMemento> _history;

        public List<ShapeDecorator> Shapes => _shapesGroup.Shapes;
        public List<ShapeDecorator> SelectedShapes => _selectedShapesGroup.Shapes;

        private CanvasHandler(ShapeDecoratorGroup shapesGroup, SelectedShapeDecoratorGroup selectedShapesGroup)
            : base(shapesGroup, selectedShapesGroup) 
        {
            _history = new List<ShapesMemento>();
        }

        public CanvasHandler()
            : this(
                ShapeDecoratorFactory.GetShapeDecoratorGroup(),
                ShapeDecoratorFactory.GetSelectedShapeDecoratorGroup())
        { }

        public void SelectShape(ShapeDecorator shape)
        {
            Select(shape);
        }

        public void ForceSelectShape(ShapeDecorator shape)
        {
            UnselectAll();
            Select(shape);
        }

        public void GroupShapes()
        {
            if (_selectedShapesGroup.Shapes.Count < 2)
            {
                return;
            }

            ShapeDecoratorGroup group = ShapeDecoratorFactory.GetShapeDecoratorGroup();
            _selectedShapesGroup.Shapes.ForEach(x => group.Add(x));

            _selectedShapesGroup.RemoveAll();
            _selectedShapesGroup.Add(group);
        }

        public void UngroupShapes()
        {
            List<ShapeDecorator> groups = _selectedShapesGroup.Shapes.ToList();
            foreach (ShapeDecorator group in groups)
            {
                if (group.GetType() == typeof(ShapeDecoratorGroup))
                {
                    foreach (ShapeDecorator shape in ((ShapeDecoratorGroup)group).Shapes)
                    {
                        _selectedShapesGroup.Add(shape);
                    }
                    _selectedShapesGroup.Remove(group);
                }
            }
        }

        public void MoveShapes(int moveX, int moveY)
        {
            _selectedShapesGroup.Move(moveX, moveY);
        }

        public override void Draw(RenderWindow window)
        {
            _shapesGroup.Draw(window);
            _selectedShapesGroup.Draw(window);
        }

        public void SaveToHistory()
        {
            ShapesMemento memento = new(_shapesGroup.Shapes, _selectedShapesGroup.Shapes);
            _history.Add(memento);
        }

        public void RollBackHistory()
        {
            if (_history.Count == 0)
            {
                return;
            }

            ShapesMemento memento = _history.Last();
            _shapesGroup.RemoveAll();
            _selectedShapesGroup.RemoveAll();
            memento.Shapes.ForEach(x => _shapesGroup.Add(x));
            memento.SelectedShapes.ForEach(x => _selectedShapesGroup.Add(x));

            _history.Remove(memento);
        }

        public void Clear()
        {
            _shapesGroup.RemoveAll();
            _selectedShapesGroup.RemoveAll();
            _history.Clear();
        }
    }
}
