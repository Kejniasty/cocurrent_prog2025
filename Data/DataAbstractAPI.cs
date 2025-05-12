using System.Numerics;

namespace Data
{
    public abstract class DataAbstractAPI
    {
        public static DataAbstractAPI CreateDataAPI()
        {
            return new DataLayer();
        }

        public virtual BallData GetBallData(Vector2 position, Vector2 velocity, float radius, float weight, int id)
        {
            return new BallData(position, velocity, radius, weight, id);
        }

        public virtual BoardData GetBoardData(int width, int height, int y_offset, int x_offset)
        {
            return new BoardData(width, height, y_offset, x_offset);
        }
    }

    internal class DataLayer : DataAbstractAPI
    {
        public DataLayer() { }
    }
}