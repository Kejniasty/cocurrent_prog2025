using System.Numerics;

namespace Data
{
    public class BallData
    {
        public Vector2 Position { get; set; }
        public float Radius { get; set; }
        public float Weight { get; set; }
        public Vector2 Velocity { get; set; }
        public int Speed { get; set; } = 1000;

        public BallData(Vector2 position, Vector2 velocity, float radius, float weight)
        {
            Position = position;
            Velocity = velocity;
            Radius = radius;
            Weight = weight;
        }
    }
}