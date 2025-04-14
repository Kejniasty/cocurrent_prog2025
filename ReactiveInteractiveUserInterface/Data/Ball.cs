//____________________________________________________________________________________________________________________________________
//
//  Copyright (C) 2024, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community by pressing the `Watch` button and get started commenting using the discussion panel at
//
//  https://github.com/mpostol/TP/discussions/182
//
//_____________________________________________________________________________________________________________________________________
// Modified by Lech Czochra
// Added HandleCollisions method

namespace TP.ConcurrentProgramming.Data
{
    internal class Ball : IBall
    {
        #region ctor
        internal Ball(Vector initialPosition, Vector initialVelocity)
        {
            PositionBackingField = initialPosition;
            Velocity = initialVelocity;
            Radius = 10.0; // Promień = 10, bo srednica to 20
        }
        #endregion ctor

        #region IBall
        public event EventHandler<IVector>? NewPositionNotification;
        public IVector Velocity { get; set; }
        public IVector Position => PositionBackingField; 
        public double Radius { get; } 
        #endregion IBall

        #region private
        private Vector PositionBackingField;

        private void RaiseNewPositionChangeNotification()
        {
            NewPositionNotification?.Invoke(this, PositionBackingField);
        }

        internal void Move(Vector delta)
        {
            double scale = 0.016;
            double newX = PositionBackingField.x + delta.x * scale;
            double newY = PositionBackingField.y + delta.y * scale;
            double diameter = 2 * Radius;
            // 420x420, test driven border
            double minX = -228 + diameter; 
            double minY = -435 + diameter;
            double maxX = 208 - diameter; 
            double maxY = 0 - diameter;
            Vector newVelocity = (Vector)Velocity;

            if (newX < minX) // Left Border
            {
                newX = minX;
                newVelocity = new Vector(-newVelocity.x, newVelocity.y);
            }
            else if (newX > maxX) // Right Border
            {
                newX = maxX;
                newVelocity = new Vector(-newVelocity.x, newVelocity.y);
            }

            if (newY < minY) // Lower Border
            {
                newY = minY;
                newVelocity = new Vector(newVelocity.x, -newVelocity.y);
            }
            else if (newY > maxY) // Upper Border
            {
                newY = maxY;
                newVelocity = new Vector(newVelocity.x, -newVelocity.y);
            }

            UpdatePosition(new Vector(newX, newY)); // Aktualizacja pozycji i prędkości
            Velocity = newVelocity;
        }

        internal void UpdatePosition(Vector newPosition)
        {
            PositionBackingField = newPosition;
            RaiseNewPositionChangeNotification();
        }
        #endregion private
    }
}