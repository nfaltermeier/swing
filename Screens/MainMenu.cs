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
            MainGame.Instance.Deaths = 0;
            MainGame.Instance.RunTime = TimeSpan.Zero;

            float halfScreenWidth = MainGame.Instance.DisplayWidth / 2f;
            float halfScreenHeight = MainGame.Instance.DisplayHeight / 2f;

            string tutorial_text = "Movement - WASD\nJump - Space (Hold for higher jump)\nInteract with menu - Mouse\nPause - Escape\n";
            string objective_text = "Get to the goal (green square)\nas fast as you can to beat the level!\nGood luck!";

            Instantiate(new TextRenderer(new Vector2(halfScreenWidth / 2, halfScreenHeight), tutorial_text, TextRenderer.Style.Medium));
            Instantiate(new TextRenderer(new Vector2(halfScreenWidth * 1.5f, halfScreenHeight), objective_text, TextRenderer.Style.Medium));
            Instantiate(new TextRenderer(new Vector2(halfScreenWidth, 200), "Swing", TextRenderer.Style.Large));
            Instantiate(new Button(new Vector2(halfScreenWidth, 400), "Play")).Activated += Play_Activated;
            Instantiate(new SettingsButton<MainMenu>(new Vector2(halfScreenWidth, 600)));
            Instantiate(new QuitButton(new Vector2(halfScreenWidth, 800)));
            Instantiate(new QuitOnMenu());
        }

        private void Play_Activated()
        {
            ScreenManager.QueueAddScreen(new MainGameScreen(1));
            ExitScreen();
        }
    }
}
