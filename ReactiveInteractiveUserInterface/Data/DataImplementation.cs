﻿//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________
// Modified by Lech Czochra
// Added HandleCollisions method, as well as modifiying the Start method

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace TP.ConcurrentProgramming.Data
{
    internal class DataImplementation : DataAbstractAPI
    {
        #region ctor
        public DataImplementation()
        {
            MoveTimer = new Timer(Move, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(1));
        }
        #endregion ctor

        #region DataAbstractAPI
        public override void Start(int numberOfBalls, Action<IVector, IBall> upperLayerHandler)
        {
            if (Disposed)
                throw new ObjectDisposedException(nameof(DataImplementation));
            if (upperLayerHandler == null)
                throw new ArgumentNullException(nameof(upperLayerHandler));
            Random random = new Random();

            // List of example possible velocities
            Vector[] possibleVelocities = new Vector[]
            {
             new Vector(90.0, 45.0),    
             new Vector(90.0, -45.0),   
             new Vector(90.0, 45.0),    
             new Vector(45.0, -90.0), 
             new Vector(45.0, 90.0),
             new Vector(-45.0, 90.0),
             new Vector(-90.0, -45.0),
             new Vector(-90.0, 45.0),
             new Vector(-45.0, -90.0)
            };

            // Create balls with random starting positions and velocities
            for (int i = 0; i < numberOfBalls; i++) 
            {
                Vector startingPosition = new(random.Next(-210, 200), random.Next(-430, 0)); 
                Vector initialVelocity = possibleVelocities[random.Next(possibleVelocities.Length)];
                Ball newBall = new(startingPosition, initialVelocity);
                upperLayerHandler(startingPosition, newBall);
                BallsList.Add(newBall);
            }
        }
        #endregion DataAbstractAPI

        #region IDisposable
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    MoveTimer.Dispose();
                    BallsList.Clear();
                }
                Disposed = true;
            }
            else
                throw new ObjectDisposedException(nameof(DataImplementation));
        }

        public override void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion IDisposable

        #region private
        private bool Disposed = false;
        private readonly Timer MoveTimer;
        private Random RandomGenerator = new();
        private List<Ball> BallsList = [];

        private void Move(object? x)
        {
            HandleCollisions();

            foreach (Ball item in BallsList.ToList())
            {
                item.Move((Vector)item.Velocity);
            }
        }

        private void HandleCollisions()
        {
            for (int i = 0; i < BallsList.Count; i++)
            {
                for (int j = i + 1; j < BallsList.Count; j++)
                {
                    Ball ball1 = BallsList[i] as Ball;
                    Ball ball2 = BallsList[j] as Ball;

                    if (ball1 == null || ball2 == null) continue;

                    double dx = ball2.Position.x - ball1.Position.x;
                    double dy = ball2.Position.y - ball1.Position.y;
                    double distance = Math.Sqrt(dx * dx + dy * dy);

                    // Balls are colliding
                    if (distance <= (ball1.Radius + ball2.Radius))
                    {

                        // Avoiding division by zero
                        if (distance == 0)
                        {
                            dx = 1.0;
                            dy = 0.0;
                            distance = 1.0;
                        }
                        double nx = dx / distance;
                        double ny = dy / distance;

                        // Calculate the relative velocity
                        double dvx = ball2.Velocity.x - ball1.Velocity.x;
                        double dvy = ball2.Velocity.y - ball1.Velocity.y;

                        // Calculate the dot product of the relative velocity and the normal vector
                        double dotProduct = dvx * nx + dvy * ny;

                        // if the dot product is negative, the balls are moving towards each other
                        if (dotProduct < 0)
                        {
                            // let's say the mass of both balls is 1
                            // we divide by 2.0 because both balls have the same mass
                            double impulse = 2 * dotProduct / 2.0;

                            // New velocities after collision
                            ball1.Velocity = new Vector(
                              ball1.Velocity.x + impulse * nx,
                              ball1.Velocity.y + impulse * ny
                            );
                            ball2.Velocity = new Vector(
                              ball2.Velocity.x - impulse * nx,
                              ball2.Velocity.y - impulse * ny
                            );

                            // The balls are overlapping, so we need to separate them
                            double overlap = (ball1.Radius + ball2.Radius - distance) / 2;
                            ball1.UpdatePosition(new Vector(
                              ball1.Position.x - overlap * nx,
                              ball1.Position.y - overlap * ny
                            ));
                            ball2.UpdatePosition(new Vector(
                              ball2.Position.x + overlap * nx,
                              ball2.Position.y + overlap * ny
                            ));
                        }
                    }
                }
            }
        }
        #endregion private

        #region TestingInfrastructure
        [Conditional("DEBUG")]
        internal void CheckBallsList(Action<IEnumerable<IBall>> returnBallsList)
        {
            returnBallsList(BallsList);
        }

        [Conditional("DEBUG")]
        internal void CheckNumberOfBalls(Action<int> returnNumberOfBalls)
        {
            returnNumberOfBalls(BallsList.Count);
        }

        [Conditional("DEBUG")]
        internal void CheckObjectDisposed(Action<bool> returnInstanceDisposed)
        {
            returnInstanceDisposed(Disposed);
        }
        #endregion TestingInfrastructure
    }
}