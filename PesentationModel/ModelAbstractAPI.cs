using Logic;
using System.Collections.ObjectModel;

namespace PesentationModel
{
    public abstract class ModelAbstractAPI
    {
        public static ModelAbstractAPI CreateModelAPI(LogicAbstractAPI logicApi = default(LogicAbstractAPI))
        {
            return new ModelAPILayer(logicApi);
        }

        public abstract int Width { get; }
        public abstract int Height { get; }

        public abstract ObservableCollection<BallLogic> CreateBalls(int ballsNumber);
        public abstract void CallSimulation();
        public abstract void StopSimulation();
        public abstract int GetBallAmount();
    }
    internal class ModelAPILayer : ModelAbstractAPI
    {
        private readonly LogicAbstractAPI logicLayer;
        public override int Width => logicLayer.Board.Width;
        public override int Height => logicLayer.Board.Height;

        public ModelAPILayer() : this(LogicAbstractAPI.CreateApi()) { }

        public ModelAPILayer(LogicAbstractAPI logicApi)
        {
            logicLayer = logicApi ?? LogicAbstractAPI.CreateApi();
        }

        public override void CallSimulation()
        {
            logicLayer.RunSimulation();
        }

        public override void StopSimulation()
        {
            logicLayer.StopSimulation();
        }

        public override ObservableCollection<BallLogic> CreateBalls(int ballsNumber)
        {
            logicLayer.CreateBalls(ballsNumber);
            return logicLayer.Balls;
        }

        public override int GetBallAmount()
        {
            return logicLayer.Balls.Count;
        }
    }
}