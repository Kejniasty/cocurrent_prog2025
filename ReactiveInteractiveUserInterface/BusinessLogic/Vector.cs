public class Vector : IVector
{
    public double x { get; set; }
    public double y { get; set; }

    public Vector(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public void Add(Vector other)
    {
        x += other.x;
        y += other.y;
    }

    public void Multiply(double scalar)
    {
        x *= scalar;
        y *= scalar;
    }
}
