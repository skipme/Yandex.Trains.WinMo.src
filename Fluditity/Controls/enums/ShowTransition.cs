using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    /// <summary>
    /// The animation types for Panel.Show and Panel.Hide.
    /// </summary>
    public enum ShowTransition
    {
        None,

        FromBottom,
        FromTop,
        FromLeft,
        FromRight,
        Zoom,
        ToLeft
    }
}
