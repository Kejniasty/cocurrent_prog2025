using System.Numerics;
using System.Collections.Generic;

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

        // New abstract method for logging
        public abstract void LogBallsState(List<(int Id, float X, float Y, Vector2 Velocity)> ballsSnapshot, string logFilePath);
    }

    internal class DataLayer : DataAbstractAPI
    {
        public DataLayer() { }

        // Implement logging in the data layer
        public override void LogBallsState(List<(int Id, float X, float Y, Vector2 Velocity)> ballsSnapshot, string logFilePath)
        {
            lock (this) // Thread-safe file access
            {
                try
                {
                    var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    using (StreamWriter writer = new StreamWriter(logFilePath, true))
                    {
                        writer.WriteLine($"Timestamp: {timestamp}");
                        foreach (var ball in ballsSnapshot)
                        {
                            writer.WriteLine($"Ball ID: {ball.Id}, Position: ({ball.X}, {ball.Y}), Velocity: ({ball.Velocity.X}, {ball.Velocity.Y})");
                        }
                        writer.WriteLine(); // Add a blank line for readability
                    }
                }
                catch (Exception ex)
                {
                    // Handle potential file access errors
                    Console.WriteLine($"Error writing to log file: {ex.Message}");
                }
            }
        }
    }
}