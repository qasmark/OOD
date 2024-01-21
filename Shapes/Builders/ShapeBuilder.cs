using Shapes.Decorators;

namespace Shapes.Builders
{
    public abstract class ShapeBuilder
    {
        public abstract void Build();
        public abstract ShapeDecorator GetResult();
    }
}