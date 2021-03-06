/*
 * FrameCounter code by craftworkgames from https://stackoverflow.com/a/20679895/5863057
 * Modified to have its own draw method
 * Licensed under CC BY-SA 3.0 https://creativecommons.org/licenses/by-sa/3.0/legalcode
 */

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Swing.Engine
{
    public class FrameCounter
    {
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private readonly Queue<float> _sampleBuffer = new Queue<float>();

        private bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Update(deltaTime);

            spriteBatch.DrawString(font, $"FPS: {AverageFramesPerSecond:F}", new Vector2(1, 1), Color.White);
        }
    }
}
