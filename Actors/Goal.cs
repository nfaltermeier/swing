using Microsoft.Xna.Framework;
using Swing.Engine;
using Swing.Screens;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace Swing.Actors
{
    class Goal : BodiedActor
    {
        public Goal(Vector2 position, int sideLength) : base(position)
        {
            Body = MainGame.Instance.World.CreateRectangle(sideLength / (MainGame.PhysicsScale * 2),
                sideLength / (MainGame.PhysicsScale * 2), 20, Position / sideLength / MainGame.PhysicsScale);
            Body.OnCollision += Body_OnCollision;
            foreach (Fixture f in Body.FixtureList)
            {
                f.IsSensor = true;
            }
        }

        private bool Body_OnCollision(Fixture sender, Fixture other, Contact contact)
        {
            if (other.Tag is ColliderTags tag)
            {
                if (tag == ColliderTags.Player)
                {
                    if (Screen is MainGameScreen mgs)
                    {
                        if (MainGameScreen.LevelExists(mgs.Level + 1))
                        {
                            Screen.ScreenManager.QueueAddScreen(new MainGameScreen(mgs.Level + 1));
                            mgs.ExitScreen();
                        }
                        else
                        {
                            Screen.ScreenManager.QueueAddScreen(new VictoryScreen());
                        }
                    }
                    else
                    {
                        Debug.LogError($"Goal expected to be on a MainGameScreen, was on a {Screen.GetType()}");
                    }
                }
            }

            return true;
        }
    }
}
