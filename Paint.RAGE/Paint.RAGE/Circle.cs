namespace Paint.RAGE
{
    // System
    using System.Drawing;

    // RPH
    using Rage;

    internal struct Circle
    {
        public readonly Vector2 Position;
        public readonly float Radius;
        public readonly Color Color;

        public Circle(Vector2 pos, float radius, Color color)
        {
            Position = pos;
            Radius = radius;
            Color = color;
        }
    }
}
