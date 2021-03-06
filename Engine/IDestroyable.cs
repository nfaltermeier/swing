using System;
using System.Collections.Generic;
using System.Text;

namespace Swing.Engine
{
    public interface IDestroyable
    {
        bool IsDestroyed { get; }

        /// <summary>
        /// Should always first check if this is already destroyed.
        /// Used to set IsDestroyed to true and inform the Game that this needs to be destroyed.
        /// </summary>
        void Destroy();

        /// <summary>
        /// Used to null references and free resources.
        /// </summary>
        void FinalDestroy();
    }
}
