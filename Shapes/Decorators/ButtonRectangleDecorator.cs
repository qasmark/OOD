using SFML.Graphics;
using SFML.System;
using Shapes.Types;
using System;

namespace Shapes.Decorators
{
    public sealed class ButtonRectangleDecorator : RectangleDecorator
    {
        private readonly ToolbarButtonType _type;
        private readonly Action _command;
        private readonly Text _text;

        private const int DefaultButtonHeight = 30;
        private const int VerticalMarginOut = 10;
        private const int HorizontMarginIn = 7;
        private const int HorizontMarginOut = 10;

        public ToolbarButtonType Type => _type;

        public ButtonRectangleDecorator(ToolbarButtonType type, Action command, string text)
            : this(new RectangleShape())
        {
            _type = type;
            _command = command;
            _text = new Text(text, DefaultFiles.Font, 18);
            Build();
        }

        private ButtonRectangleDecorator(RectangleShape shape)
            : base(shape) { }

        public void Press() => _command();

        public override void Draw(RenderWindow window)
        {
            base.Draw(window);
            window.Draw(_text);
        }

        public override void Move(int moveX, int moveY)
        {
            base.Move(moveX, moveY);
            MoveText(moveX, moveY);
        }

        private void MoveText(int moveX, int moveY)
        {
            Vector2f currentPosition = _text.Position;
            _text.Position = new Vector2f(currentPosition.X + moveX, currentPosition.Y + moveY);
        }

        private void Build()
        {
            SetPosition(new Vector2f(HorizontMarginOut, VerticalMarginOut));
            SetSize(new Vector2f(_text.GetGlobalBounds().Width + HorizontMarginIn * 2, DefaultButtonHeight));
            SetFillColor(DefaultColors.DeepDark);

            int textHeight = (int)_text.GetGlobalBounds().Height;
            MoveText(HorizontMarginIn + HorizontMarginOut, VerticalMarginOut + (DefaultButtonHeight - textHeight) / 4);
        }
    }
}
