using Logic;
using System.Numerics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest
{
    [TestClass]
    public class BallTests
    {
        private LogicAbstractAPI _logicApi;
        [TestInitialize]
        public void SetUp()
        {
            _logicApi = LogicAbstractAPI.CreateApi();
        }

        [TestMethod]
        public void BallContructorTest()
        {
            Vector2 v = new Vector2(1, 2);
            int radius = 5;
            Ball ball = new Ball(v, radius);

            Assert.AreEqual(radius, ball.Radius);
            Assert.AreEqual(v, ball.Position);

        }

        [TestMethod]
        public void PositionChangedTest()
        {
            Ball ball = new Ball();
            ball.Velocity = new Vector2(1, 2);
            ball.Position = new Vector2(_logicApi.Width, _logicApi.Height);
            ball.ChangePosition();
            Assert.AreNotEqual(_logicApi.Width, ball.Velocity.X);
            Assert.AreNotEqual(_logicApi.Height, ball.Velocity.Y);
        }

        [TestMethod]
        public void BallVelocityTest()
        {
            Ball ball = new Ball();
            ball.Velocity = new Vector2(1, 2);
            Assert.AreEqual(1, ball.Velocity.X);
            Assert.AreEqual(2, ball.Velocity.Y);
        }


        [TestMethod]
        public void LogicApiConstructorTest()
        {
            int _width = 750;
            int _height = 400;
            Assert.AreEqual(_width, _logicApi.Width);
            Assert.AreEqual(_height, _logicApi.Height);
            Assert.AreEqual(_logicApi.Balls.Count, 0);
        }

        [TestMethod]
        public void CreateBallsTest()
        {
            int _amount = 5;
            int _radius = 25;
            _logicApi.CreateBalls(_amount, _radius);

            Assert.AreEqual(_amount, _logicApi.Balls.Count);

            foreach (Ball ball in _logicApi.Balls)
            {
                Assert.IsTrue(ball.Position.X >= 1);
                Assert.IsTrue(ball.Position.X <= _logicApi.Width - _radius);
                Assert.IsTrue(ball.Position.Y >= 1);
                Assert.IsTrue(ball.Position.Y <= _logicApi.Height - _radius);
            }

        }

        [TestMethod]
        public void DeleteBallsTest()
        {
            _logicApi.DeleteBalls();
            Assert.AreEqual(0, _logicApi.Balls.Count);
        }

    }
}
