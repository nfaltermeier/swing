using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Swing.Engine.Actors.Particles
{
    public struct Particle
    {
        public Vector2 Position;

        public Vector2 Velocity;

        public Vector2 Acceleration;

        public float Rotation;

        public float AngularVelocity;

        public float AngularAcceleration;

        public float Scale;

        public float Lifetime;

        public float TimeSinceStart;

        public Color Color;

        public bool Active => TimeSinceStart < Lifetime;


        public void Initialize(Vector2 where, float lifetime = 1, float scale = 1, float rotation = 0, float angularVelocity = 0, float angularAcceleration = 0)
        {
            this.Position = where;
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
            this.Rotation = rotation;
            this.AngularVelocity = angularVelocity;
            this.AngularAcceleration = angularAcceleration;
            this.Scale = scale;
            this.Color = Color.White;
            this.Lifetime = lifetime;
            this.TimeSinceStart = 0f;
        }

        public void Initialize(Vector2 position, Vector2 velocity, float lifetime = 1, float scale = 1, float rotation = 0, float angularVelocity = 0, float angularAcceleration = 0)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = Vector2.Zero;
            this.Lifetime = lifetime;
            this.TimeSinceStart = 0f;
            this.Scale = scale;
            this.Rotation = rotation;
            this.AngularVelocity = angularVelocity;
            this.AngularAcceleration = angularAcceleration;
            this.Color = Color.White;
        }

        public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration, float lifetime = 1, float scale = 1, float rotation = 0, float angularVelocity = 0, float angularAcceleration = 0)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Lifetime = lifetime;
            this.TimeSinceStart = 0f;
            this.Scale = scale;
            this.Rotation = rotation;
            this.AngularVelocity = angularVelocity;
            this.AngularAcceleration = angularAcceleration;
            this.Color = Color.White;
        }

        public void Initialize(Vector2 position, Vector2 velocity, Vector2 acceleration, Color color, float lifetime = 1, float scale = 1, float rotation = 0, float angularVelocity = 0, float angularAcceleration = 0)
        {
            this.Position = position;
            this.Velocity = velocity;
            this.Acceleration = acceleration;
            this.Lifetime = lifetime;
            this.TimeSinceStart = 0f;
            this.Scale = scale;
            this.Rotation = rotation;
            this.AngularVelocity = angularVelocity;
            this.AngularAcceleration = angularAcceleration;
            this.Color = color;
        }
    }
}
