﻿using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using Data;
using Logic;

public class BallLogic : INotifyPropertyChanged
{
    private readonly BallData _ballData;

    public Vector2 Velocity 
    { 
        get => _ballData.Velocity;
        set 
        {
            _ballData.Velocity = value;
            RaisePropertyChanged(nameof(Velocity));
        }
    }

    public BallLogic(BallData ballData)
    {
        _ballData = ballData;
    }

    public float X => _ballData.Position.X;
    public float Y => _ballData.Position.Y;
    public float DrawX => _ballData.Position.X - _ballData.Radius / 2f;
    public float DrawY => _ballData.Position.Y - _ballData.Radius / 2f;
    public float Radius => _ballData.Radius;
    public float Weight => _ballData.Weight;
    public int Id => _ballData.id;
    private static BoardData _boardData;

    public static void SetBoardData(BoardData boardData)
    {
        _boardData = boardData;
    }

    public bool CollidesWith(BallLogic other)
    {
        Vector2 distance = new Vector2(other.X, other.Y) - new Vector2(this.X, this.Y);
        float radiiSum = this.Radius / 2  + other.Radius / 2;

        return distance.LengthSquared() <= radiiSum * radiiSum;
    }


    public void HandleCollision(BallLogic other)
    {
        Vector2 posA = new Vector2(this.X, this.Y);
        Vector2 posB = new Vector2(other.X, other.Y);
        Vector2 delta = posB - posA;
        float distance = delta.Length();
        float minDistance = (this.Radius + other.Radius) / 2f;

        // Prevent division by zero
        if (distance == 0f)
            return;


        Vector2 collisionNormal = Vector2.Normalize(delta);
        Vector2 relativeVelocity = other.Velocity - this.Velocity;

        float restitution = 1.0f; // Fully elastic collisions
        float velocityAlongNormal = Vector2.Dot(relativeVelocity, collisionNormal);

        if (velocityAlongNormal > 0) return; // Balls are moving apart

        float invMassA = 1f / this.Weight;
        float invMassB = 1f / other.Weight;

        float impulseMagnitude = -(1 + restitution) * velocityAlongNormal / (invMassA + invMassB);
        Vector2 impulse = impulseMagnitude * collisionNormal;

        this.Velocity -= impulse * invMassA;
        other.Velocity += impulse * invMassB;
    }



    public event PropertyChangedEventHandler PropertyChanged;

    public void ChangePosition()
    {
        _ballData.Position += new Vector2(_ballData.Velocity.X * _ballData.Speed, _ballData.Velocity.Y * _ballData.Speed);
        Vector2 normal = Vector2.Zero;

        if (_ballData.Position.X - _ballData.Radius / 2f < _boardData.XOffset)
        {
            normal = Vector2.UnitX;
        }
        else if (_ballData.Position.X + _ballData.Radius / 2f > _boardData.Width)
        {
            normal = -Vector2.UnitX;
        }

        if (_ballData.Position.Y - _ballData.Radius / 2f < _boardData.YOffset)
        {
            normal = Vector2.UnitY;
        }
        else if (_ballData.Position.Y + _ballData.Radius / 2f > _boardData.Height)
        {
            normal = -Vector2.UnitY;
        }


        if (normal != Vector2.Zero)
             _ballData.Velocity = Vector2.Reflect(_ballData.Velocity, normal);
        RaisePropertyChanged(nameof(DrawX));
        RaisePropertyChanged(nameof(DrawY));
    }


    public void SetVelocity(Vector2 velocity)
    {
        Velocity = velocity;
    }

    protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}