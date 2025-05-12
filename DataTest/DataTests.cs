using NUnit.Framework;
using System.Numerics;
using Data;

namespace Tests
{
    public class DataTests
    {
        [Test]
        public void GetBallData_ReturnsCorrectObject()
        {
            // Arrange
            var dataAPI = DataAbstractAPI.CreateDataAPI();
            var position = new Vector2(10, 20);
            var velocity = new Vector2(0, 1);
            var radius = 5;
            var weight = 2.5f;

            // Act
            var ballData = dataAPI.GetBallData(position, velocity, radius,weight, 1);

            // Assert
            Assert.AreEqual(position, ballData.Position);
            Assert.AreEqual(velocity, ballData.Velocity);
            Assert.AreEqual(radius, ballData.Radius);
            Assert.AreEqual(1000, ballData.Speed); // default value
        }

        [Test]
        public void GetBoardData_ReturnsCorrectObject()
        {
            // Arrange
            var dataAPI = DataAbstractAPI.CreateDataAPI();
            var width = 800;
            var height = 600;
            var Yoffset = -20;
            var Xoffset = -10;

            // Act
            var boardData = dataAPI.GetBoardData(width, height, Yoffset, Xoffset);

            // Assert
            Assert.AreEqual(width, boardData.Width);
            Assert.AreEqual(height, boardData.Height);
            Assert.AreEqual(Yoffset, boardData.YOffset);
            Assert.AreEqual(Xoffset, boardData.XOffset);
        }
    }
}