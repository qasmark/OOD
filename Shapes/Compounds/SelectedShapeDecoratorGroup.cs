using SFML.Graphics;
using SFML.System;
using Shapes.Decorators;

namespace Shapes.Compounds
{
    public sealed class SelectedShapeDecoratorGroup : ShapeDecoratorGroup
    {
        public SelectedShapeDecoratorGroup()
            : base() { }

        public override void Draw(RenderWindow window)
        {
            if (base.Shapes.Count == 0)
            {
                return;
            }

            IntRect bounds = GetGlobalBounds();
            RectangleDecorator frame = new(new RectangleShape());

            frame.SetPosition(new Vector2f(bounds.Left, bounds.Top));
            frame.SetSize(new Vector2f(bounds.Width, bounds.Height));
            frame.SetFillColor(new Color(0, 0, 0, 0));
            frame.SetOutlineThickness(1);
            frame.SetOutlineColor(Color.White);

            base.Draw(window);
            frame.Draw(window);
        }
    }
}
