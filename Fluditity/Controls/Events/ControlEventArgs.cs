using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class ControlEventArgs:EventArgs
    {
        public ControlEventArgs(FluidControl c)
        {
            Control = c;
        }

        public FluidControl Control { get; private set; }
    }
}
