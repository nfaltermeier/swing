using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace Swing.Engine
{
    public abstract class BodiedActor : Actor
    {
        private Body _body;
        public Body Body {
            get => _body;
            protected set
            {
                Vector2 pos = Position;
                _body = value;
                Position = pos;
            }
        }

        public override Vector2 Position {
            get
            {
                if (Body != null)
                    return Body.Position;
                else
                    return base.Position;
            }

            set
            {
                if (Body != null)
                    Body.Position = value;
                else
                    base.Position = value;
            }
        }

        public override void FinalDestroy()
        {
            base.FinalDestroy();

            if (Body != null)
            {
                Game.Instance.World.Remove(Body);
                Body = null;
            }
        }
    }
}
