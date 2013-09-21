using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class PointEventArgs : EventArgs
    {
        internal PointEventArgs()
            : base()
        {
        }

        public PointEventArgs(Gesture gesture, int x, int y)
            : base()
        {
            this.X = x;
            this.Y = y;
            this.Gesture = gesture;
        }
        public int X { get; internal set; }
        public int Y { get; internal set; }
        public Gesture Gesture { get; internal set; }
    }
}
