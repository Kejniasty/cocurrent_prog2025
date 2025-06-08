using System.Collections.ObjectModel;
using System.Numerics;
using Data;
using System.Timers;
using System;
using System.IO;
using System.Threading;

namespace Logic
{
    internal class LogicApi : LogicAbstractAPI
    {
        private readonly DataAbstractAPI _dataAPI;
        private readonly object _lock = new object();
        private readonly System.Timers.Timer _logTimer;
        private readonly string _logFilePath = "simulation_log.txt";

        public override BoardData Board { get; }
        public override ObservableCollection<BallLogic> Balls { get; } = new ObservableCollection<BallLogic>();

        private CancellationTokenSource _cancelTokenSource;
        private List<Task> _tasks = new List<Task>();
        private Task _loggingTask; // Dedicated task for logging


        public LogicApi()
        {
            _dataAPI = DataAbstractAPI.CreateDataAPI();
            Board = _dataAPI.GetBoardData(496, 497, 3, 3);

            // Pass the BoardData instance to BallLogic using BallLogic.SetBoardData
            BallLogic.SetBoardData(Board);
        }

        private async Task RunLoggingLoop(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    List<(int, float, float, Vector2)> ballsSnapshot;
                    lock (_lock)
                    {
                        // Create a snapshot of ball data to pass to the data layer
                        ballsSnapshot = Balls.Select(ball => (ball.Id, ball.X, ball.Y, ball.Velocity)).ToList();
                    }
                    _dataAPI.LogBallsState(ballsSnapshot, _logFilePath);
                    // Wait for 10 seconds before the next log
                    await Task.Delay(10000, cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in logging task: {ex.Message}");
            }
        }
           

        public override void RunSimulation()
        {
            _cancelTokenSource = new CancellationTokenSource();

            // Start the logging task on a dedicated thread
            _loggingTask = Task.Factory.StartNew(
                () => RunLoggingLoop(_cancelTokenSource.Token).GetAwaiter().GetResult(),
                _cancelTokenSource.Token,
                TaskCreationOptions.LongRunning, 
                TaskScheduler.Default
            );
            _tasks.Add(_loggingTask);

            // Add Barrier object and set initial count to number of balls
            Barrier barrier = new Barrier(Balls.Count);

            foreach (BallLogic ball in Balls)
            {
                Task task = Task.Run(() =>
                {
                    // Wait for all balls to start updating before continuing
                    barrier.SignalAndWait();

                    while (true)
                    {
                        Thread.Sleep(4);

                        try
                        {
                            _cancelTokenSource.Token.ThrowIfCancellationRequested();
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }

                        // Create a local copy of the Balls collection to avoid locking
                        var ballsSnapshot = Balls.ToList();

                        foreach (BallLogic otherBall in ballsSnapshot)
                        {
                            if (ball == otherBall) continue;

                            // Ensure only one direction of the collision is handled
                            if (ball.Id < otherBall.Id && ball.CollidesWith(otherBall))
                            {
                                ball.HandleCollision(otherBall);
                            }
                        }

                        ball.ChangePosition();
                    }
                }, _cancelTokenSource.Token);

                _tasks.Add(task);
            }
        }

        public override void StopSimulation()
        {
            // Stop the logging timer
            _logTimer.Stop();

            // Cancel the simulation tasks
            _cancelTokenSource?.Cancel();

            foreach (Task task in _tasks)
            {
                task.Wait();
            }

            _tasks.Clear();
            Balls.Clear();

            // Dispose of the cancellation token source
            _cancelTokenSource?.Dispose();
            _cancelTokenSource = null;
        }

        public override BallLogic CreateBall(Vector2 pos, int radius, int id)
        {
            BallData ballData =
                _dataAPI.GetBallData(pos, new Vector2((float)0.0034, (float)0.0034), radius, radius / 2, id);
            BallLogic ballLogic = new BallLogic(ballData);
            Balls.Add(ballLogic);

            return ballLogic;
        }

        public override void CreateBalls(int count)
        {
            var rnd = new Random((int)DateTime.Now.Ticks);

            for (int i = 0; i < count; i++)
            {
                float speed = 0.0005f;
                float radius = GenerateRandomFloatInRange(rnd, 10f, 30f);
                Vector2 pos = GenerateRandomVector2InRange(rnd, radius, Board.Width - radius, radius, Board.Height - radius);
                Vector2 vel = GenerateRandomVector2InRange(rnd, -speed, speed, -speed, speed);
                BallData ballData = _dataAPI.GetBallData(pos, vel, radius, radius / 2, i);
                BallLogic ballLogic = new BallLogic(ballData);
                Balls.Add(ballLogic);
            }
        }

        public override void DeleteBalls()
        {
            Balls.Clear();
        }

        public static float GenerateRandomFloatInRange(Random random, float minValue, float maxValue)
        {
            return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
        }

        public static Vector2 GenerateRandomVector2InRange(Random random, float minValue1, float maxValue1,
            float minValue2, float maxValue2)
        {
            return new Vector2(GenerateRandomFloatInRange(random, minValue1, maxValue1),
                GenerateRandomFloatInRange(random, minValue2, maxValue2));
        }
    }
}