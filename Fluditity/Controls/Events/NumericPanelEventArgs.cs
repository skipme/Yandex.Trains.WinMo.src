using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class NumericPanelEventArgs:EventArgs
    {
        public bool Handled { get; set; }
        public int ButtonIndex { get; internal set; }
    }
}
