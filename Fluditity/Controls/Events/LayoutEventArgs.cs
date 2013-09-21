using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Fluid.Controls
{
    public class LayoutEventArgs:EventArgs
    {
        public LayoutEventArgs(SizeF scaleFactor)
            : base()
        {
            this.ScaleFactor = scaleFactor;
        }

        public SizeF ScaleFactor { private get; set; }
    }
}
