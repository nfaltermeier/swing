using Microsoft.Xna.Framework;
using Swing.Engine.Actors.UI;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class MainMenu : GameScreen
    {
        public MainMenu()
        {
            float halfScreenWidth = MainGame.Instance.DisplayWidth / 2f;
            Instantiate(new TextRenderer(new Vector2(halfScreenWidth, 200), "Swing"));
            Button play = Instantiate(new Button(new Vector2(halfScreenWidth, 400), "Play"));
            play.Activated += Play_Activated;
        }

        private void Play_Activated()
        {
            ScreenManager.AddScreen(new MainGameScreen(), null);
            ExitScreen();
        }
    }
}
