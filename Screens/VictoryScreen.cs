using Microsoft.Xna.Framework;
using Swing.Actors;
using Swing.Engine;
using Swing.Engine.Actors.UI;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class VictoryScreen : GameScreen
    {
        public VictoryScreen()
        {
            float halfWidth = MainGame.Instance.DisplayWidth / 2f;

            Instantiate(new Overlay());
            Instantiate(new TextRenderer(new Vector2(halfWidth, 200), "You win!", TextRenderer.Style.Large));
            Instantiate(new TextRenderer(new Vector2(halfWidth, 250), "Thanks for playing."));
            Instantiate(new TextRenderer(new Vector2(halfWidth, 300), $"You died {MainGame.Instance.Deaths} times."));
            Instantiate(new TextRenderer(new Vector2(halfWidth, 350),
                $"You took {MainGame.Instance.RunTime.ToString(RunTimeDisplay.format)}."));
            Instantiate(new Button(new Vector2(halfWidth, 500), "Main Menu")).Activated += MainMenuButton_Activated;
        }

        private void MainMenuButton_Activated()
        {
            ExitScreen();
            if (ScreenManager.GetScreen<MainGameScreen>() is MainGameScreen mgs)
            {
                mgs.ExitScreen();
            }

            ScreenManager.QueueAddScreen(new MainMenu());
        }

        protected override void Activate()
        {
            base.Activate();

            Time.TimeScale = 0;
        }

        internal override void Unload()
        {
            base.Unload();

            Time.TimeScale = 1;
        }
    }
}
