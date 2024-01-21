using Shapes.Decorators;
using System.Collections.Generic;

namespace Shapes.Mementos
{
    public sealed class ShapesMemento
    {
        private readonly List<ShapeDecorator> _shapes;
        private readonly List<ShapeDecorator> _selectedShapes;

        public List<ShapeDecorator> Shapes => _shapes;
        public List<ShapeDecorator> SelectedShapes => _selectedShapes;

        public ShapesMemento(
            List<ShapeDecorator> shapes,
            List<ShapeDecorator> selectedShapes)
        {
            _shapes = new List<ShapeDecorator>();
            _selectedShapes = new List<ShapeDecorator>();
            shapes.ForEach(x => _shapes.Add(x.Clone()));
            selectedShapes.ForEach(x => _selectedShapes.Add(x.Clone()));
        }
    }
}
