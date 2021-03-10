using Microsoft.Xna.Framework;
using Swing.Engine;
using Swing.Engine.Actors;
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
            Instantiate(new Button(new Vector2(halfScreenWidth, 400), "Play")).Activated += Play_Activated;
            Instantiate(new SettingsButton<MainMenu>(new Vector2(halfScreenWidth, 600)));
            Instantiate(new QuitButton(new Vector2(halfScreenWidth, 800)));
            Instantiate(new QuitOnMenu());
        }

        private void Play_Activated()
        {
            ScreenManager.QueueAddScreen(new MainGameScreen());
            ExitScreen();
        }
    }
}
