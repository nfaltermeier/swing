using Microsoft.Xna.Framework;
using Swing.Engine.Actors.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Actors
{
    public class RunTimeDisplay : TextRenderer
    {
        public static readonly string format = "hh\\:mm\\:ss\\.ff";
        public RunTimeDisplay(Vector2 position) : base(position, "")
        {

        }

        protected override void Update()
        {
            base.Update();
            text = MainGame.Instance.RunTime.ToString(format);
        }
    }
}
