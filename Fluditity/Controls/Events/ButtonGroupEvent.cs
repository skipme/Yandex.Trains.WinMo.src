using System;

using System.Collections.Generic;
using System.Text;
using Fluid.Controls.Classes;

namespace Fluid.Controls
{
    public class ButtonGroupEventArgs:EventArgs
    {
        public int Index { get; internal set; }
        public FluidButton Button { get; internal set; }
    }
}
