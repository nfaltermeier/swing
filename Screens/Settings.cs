using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Swing.Engine.Actors;
using Swing.Engine.Actors.UI;
using Swing.Engine.StateManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Screens
{
    class Settings<ScreenToReturnTo> : GameScreen where ScreenToReturnTo : GameScreen
    {
        public Settings(bool drawOverlay)
        {
            float halfWidth = MainGame.Instance.DisplayWidth / 2f;
            float thirdWidth = MainGame.Instance.DisplayWidth / 3f;
            float thirdHeight = MainGame.Instance.DisplayHeight / 3f;

            if (drawOverlay)
                Instantiate(new Overlay());

            Instantiate(new TextRenderer(new Vector2(halfWidth, 200), "Settings", TextRenderer.Style.Large));

            Instantiate(new TextRenderer(new Vector2(thirdWidth, thirdHeight - 30), "Sound Effect Volume"));
            Instantiate(new ValueSlider(new Vector2(thirdWidth, thirdHeight), SoundEffect.MasterVolume)).ValueChanged += SoundEffectVolume_ValueChanged;

            Instantiate(new TextRenderer(new Vector2(thirdWidth * 2, thirdHeight - 30), "Music Volume"));
            Instantiate(new ValueSlider(new Vector2(thirdWidth * 2, thirdHeight), MediaPlayer.Volume)).ValueChanged += MediaVolume_ValueChanged;

            Instantiate(new BackButton<ScreenToReturnTo>(new Vector2(halfWidth, thirdHeight * 2)));
            Instantiate(new GoodbyeScreenOnMenu<ScreenToReturnTo>());
        }

        private void SoundEffectVolume_ValueChanged(float value)
        {
            SoundEffect.MasterVolume = value;
        }

        private void MediaVolume_ValueChanged(float value)
        {
            MediaPlayer.Volume = value;
        }
    }
}
