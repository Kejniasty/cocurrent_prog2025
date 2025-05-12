using Data;
using Logic;
using NUnit.Framework;
using System.Collections.ObjectModel;
using System.Numerics;
using PesentationModel;

namespace PresentationModel.Tests
{
    [TestFixture]
    public class ModelAbstractAPITests
    {
        private ModelAbstractAPI model;
        private LogicAbstractAPI logicMock;

        [SetUp]
        public void Initialize()
        {
            // Create a new instance of ModelAbstractAPI with a mock of the LogicAbstractAPI
            logicMock = new LogicMock();
            model = ModelAbstractAPI.CreateModelAPI(logicMock);
        }

        [Test]
        public void Width_ReturnsExpectedValue()
        {
            int expected = 500;
            int actual = model.Width;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Height_ReturnsExpectedValue()
        {
            int expected = 500;
            int actual = model.Height;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CreateBalls_ReturnsNonNullObservableCollectionWithExpectedCount()
        {
            int expectedCount = 5;
            int radius = 10;
            ObservableCollection<BallLogic> balls = model.CreateBalls(expectedCount);

            Assert.IsNotNull(balls);
            Assert.AreEqual(expectedCount, balls.Count);
        }
        
        [Test]
        public void GetBallAmount_ReturnsExpectedValue()
        {
            int expectedCount = 3;
            logicMock.Balls.Add(new BallLogic(new BallData(Vector2.Zero, Vector2.Zero, 0,0)));
            logicMock.Balls.Add(new BallLogic(new BallData(Vector2.Zero, Vector2.Zero, 0,0)));
            logicMock.Balls.Add(new BallLogic(new BallData(Vector2.Zero, Vector2.Zero, 0,0)));

            int actualCount = model.GetBallAmount();
            Assert.AreEqual(expectedCount, actualCount);
        }

        // Mock class for LogicAbstractAPI
        // Mock class for LogicAbstractAPI
        private class LogicMock : LogicAbstractAPI
        {
            public bool RunSimulationCalled { get; set; }
            public  bool StopSimulationCalled { get; set; }
            public override ObservableCollection<BallLogic> Balls { get; } = new ObservableCollection<BallLogic>();

            public override BoardData Board => new BoardData(500, 500, -20, -10);

            public override BallLogic CreateBall(Vector2 pos, int radius)
            {
                throw new System.NotImplementedException();
            }

            public override void CreateBalls(int count)
            {
                for (int i = 0; i < count; i++)
                {
                    Balls.Add(new BallLogic(new BallData(Vector2.Zero, Vector2.Zero, 0,0)));
                }
            }

            public override void DeleteBalls()
            {
                throw new System.NotImplementedException();
            }

            public override void RunSimulation()
            {
                RunSimulationCalled = true;
            }

            public override void StopSimulation()
            {
                StopSimulationCalled = true;
            }
        }
    }
}
