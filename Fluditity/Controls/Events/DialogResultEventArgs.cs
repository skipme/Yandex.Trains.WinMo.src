using System;

using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Fluid.Classes
{
    public class DialogEventArgs:EventArgs
    {
        public DialogResult Result { get; internal set; }
    }
}
