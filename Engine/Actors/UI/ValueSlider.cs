using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine.Actors.UI
{
    class ValueSlider : Actor
    {
        private Texture2D background;
        private Color fillColor = Color.Coral;
        private Texture2D handle;

        private Rectangle backgroundLocation;
        private Rectangle handleLocation;
        private Rectangle totalFillSpace;
        private Rectangle fillLocation;

        private bool lastClicked;
        private bool mouseAttached;
        private float value;

        public override RenderType RenderType => RenderType.UI;

        public override Vector2 Position {
            get => base.Position;
            set
            {
                base.Position = value;
                if (background != null && handle != null)
                {
                    // Center on Position
                    backgroundLocation = new Rectangle((int)(value.X - background.Width / 2f), (int)(value.Y - background.Height / 2f), background.Width, background.Height);
                    // Works because these are structs
                    totalFillSpace = backgroundLocation;
                    totalFillSpace.Inflate(-2, -2);
                    CalcHandleAndFillLocation();
                }
            }
        }

        public event Action<float> ValueChanged;

        /// <summary>
        /// Takes a screen space position
        /// </summary>
        /// <param name="position">a screen space position</param>
        /// <param name="initialValue">Must be between 0 and 1</param>
        public ValueSlider(Vector2 position, float initialValue) : base(position)
        {
            value = MathHelper.Clamp(initialValue, 0, 1);
        }

        protected override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            background = content.Load<Texture2D>("Engine/UI/ValueSliderBackground");
            handle = content.Load<Texture2D>("Engine/UI/ValueSliderHandle");
            // Needed to calculate rectangles
            Position = Position;
        }

        protected override void Update()
        {
            base.Update();

            if (!mouseAttached && lastClicked && InputManager.MouseHeld)
                mouseAttached = true;
            else if (!InputManager.MouseHeld)
                mouseAttached = false;

            if (mouseAttached)
            {
                CalcNewValue();
            }

            lastClicked = false;
            if (InputManager.MousePressed && backgroundLocation.Contains(InputManager.MouseLocation))
            {
                CalcNewValue();
                lastClicked = true;
            }
        }

        protected override void Draw()
        {
            base.Draw();

            RenderSpriteUI(backgroundLocation, background, Color.White, RenderOrder.UIBack);
            RenderSpriteUI(fillLocation, Screen.ScreenManager.DebugPixel, fillColor, RenderOrder.UIMiddle);
            RenderSpriteUI(handleLocation, handle, Color.White, RenderOrder.UIFront);

            if (Debug.DISPLAY_UI_RECTANGLES)
                Debug.DrawRectangleOutlineScreenspace(handleLocation);
        }

        private void CalcHandleAndFillLocation()
        {
            float filledAmount = value * totalFillSpace.Width;
            handleLocation =
                new Rectangle((int)(totalFillSpace.Left + filledAmount - (handle.Width / 2f)),
                backgroundLocation.Y, handle.Width, handle.Height);
            // Works because these are structs
            fillLocation = totalFillSpace;
            fillLocation.Width = (int)filledAmount;
        }

        private void CalcNewValue()
        {
            float x = MathHelper.Clamp(InputManager.MouseLocation.X, totalFillSpace.Left, totalFillSpace.Right);
            float newValue = (x - totalFillSpace.Left) / totalFillSpace.Width;

            if (newValue != value)
            {
                value = newValue;
                CalcHandleAndFillLocation();
                ValueChanged?.Invoke(value);
            }
        }
    }
}
