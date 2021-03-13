using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Swing.Engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing
{
    static class InputManager
    {
        public static Vector2 Direction { get; private set; }
        public static Vector2 SecondaryDirection { get; private set; }
        public static bool MenuButton { get; private set; }
        /// <summary>
        /// The screenspace position of the mouse cursor
        /// </summary>
        public static Vector2 MouseLocation { get; private set; }
        public static bool MousePressed { get; private set; }
        public static bool MouseReleased { get; private set; }
        public static bool MouseHeld { get; private set; }
        public static bool Jump { get; private set; }

        private static KeyboardState currentKeyboardState;
        private static KeyboardState previousKeyboardState;
        private static GamePadState currentGamePadState;
        private static GamePadState previousGamePadState;
        private static MouseState currentMouseState;
        private static MouseState previousMouseState;

        public static void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            previousGamePadState = currentGamePadState;
            currentGamePadState = GamePad.GetState(0);
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            #region Direction
            Direction = currentGamePadState.ThumbSticks.Right;

            if (currentKeyboardState.IsKeyDown(Keys.A))
            {
                Direction += new Vector2(-1, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.D))
            {
                Direction += new Vector2(1, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.W))
            {
                Direction += new Vector2(0, 1);
            }

            if (currentKeyboardState.IsKeyDown(Keys.S))
            {
                Direction += new Vector2(0, -1);
            }
            #endregion

            #region SecondaryDirection
            SecondaryDirection = currentGamePadState.ThumbSticks.Left;

            if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                SecondaryDirection += new Vector2(-1, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                SecondaryDirection += new Vector2(1, 0);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                SecondaryDirection += new Vector2(0, 1);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                SecondaryDirection += new Vector2(0, -1);
            }
            #endregion

            #region MenuButton
            MenuButton = (currentGamePadState.Buttons.Start == ButtonState.Pressed && previousGamePadState.Buttons.Start == ButtonState.Released) ||
                (currentKeyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape));
            #endregion

            #region MouseLocation
            MouseLocation = currentMouseState.Position.ToVector2();
            #endregion

            #region MousePressed
            MousePressed = currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released;
            #endregion

            #region MouseReleased
            MouseReleased = currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed;
            #endregion

            #region MouseHeld
            MouseHeld = currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Pressed;
            #endregion

            #region Jump
            Jump = (currentGamePadState.Buttons.A == ButtonState.Pressed) ||
                (currentKeyboardState.IsKeyDown(Keys.Space));
            #endregion
        }
    }
}
