using System.Numerics;

namespace Data
{
    public class BallData
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Weight { get; set; }
        public Vector2 Velocity { get; set; }
        public int Speed { get; set; } = 1500;
        public int id { get; set; }

        public Vector2 DrawPosition { get => new Vector2(Position.X - Radius / 2, Position.Y - Radius / 2); }

        public BallData(Vector2 position, Vector2 velocity, float radius, float weight, int id)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Weight = weight;
            this.id = id;

        }
    }
}