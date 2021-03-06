using Swing.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Components
{
    public class RectangleCollider : Component
    {
        private Texture2D pixel;

        /// <summary>
        /// Top left corner of the rectangle
        /// </summary>
        public float X;
        /// <summary>
        /// Top left corner of the rectangle
        /// </summary>
        public float Y;
        private float _width;
        public float HalfWidth { get; private set; }
        public float Width
        {
            get => _width;
            set
            {
                _width = value;
                HalfWidth = _width / 2;
            }
        }
        private float _height;
        public float HalfHeight { get; private set; }
        public float Height
        {
            get => _height;
            set
            {
                _height = value;
                HalfHeight = _height / 2;
            }
        }

        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y - Height;

        /// <summary>
        /// position is the top left corner of the rectangle
        /// </summary>
        public RectangleCollider(Actor attached, Vector2 center, float width, float height) : base(attached)
        {
            Width = width;
            Height = height;
            SetPositionFromCenterPoint(center);
        }

        internal override void Start()
        {
            base.Start();

            Game.Instance.AddCollider(this);
        }

        public void SetPositionFromCenterPoint(Vector2 center)
        {
            X = center.X - HalfWidth;
            Y = center.Y + HalfHeight;
        }

        /// <summary>
        /// position is the top left corner of the rectangle
        /// </summary>
        public void UpdateDimensions(Vector2 center, Texture2D sprite)
        {
            Width = sprite.Width;
            Height = sprite.Height;
            SetPositionFromCenterPoint(center);
        }

        public bool CollidesWith(RectangleCollider other)
        {
            return CollisionHelper.Collides(this, other);
        }

        public void DoCollisionTriggers(RectangleCollider other)
        {
            Attached.OnTriggerEnter(this, other.Attached, other);
            if (other.Attached != null)
                other.Attached.OnTriggerEnter(other, Attached, this);
        }

        internal override void FixedUpdate()
        {
            base.FixedUpdate();

            SetPositionFromCenterPoint(Attached.Position);
        }

        internal override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            pixel = content.Load<Texture2D>("1x1WhitePixel");
        }

        internal override void Draw()
        {
            base.Draw();

            if (Debug.DISPLAY_COLLIDERS)
            {
                int iX = (int)(X);
                int iY = (int)(Y);
                int iW = (int)Width;
                int iH = (int)Height;

                Attached.RenderSprite(new Rectangle(iX, iY, 1, iH), pixel, Color.LightGreen, 1f);
                Attached.RenderSprite(new Rectangle(iX, iY, iW, 1), pixel, Color.LightGreen, 1f);
                Attached.RenderSprite(new Rectangle(iX + iW, iY, 1, iH), pixel, Color.LightGreen, 1f);
                Attached.RenderSprite(new Rectangle(iX, iY - iH, iW, 1), pixel, Color.LightGreen, 1f);
            }
        }
    }
}
