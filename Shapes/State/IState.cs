using SFML.System;

namespace Shapes.State
{
    public interface IState
    {
        void SimpleClick(Vector2i click);
        void PressedKeyG();
        void PressedKeyU();
        void MouseMoved(Vector2i mousePosition);
        void MouseReleased();
    }
}