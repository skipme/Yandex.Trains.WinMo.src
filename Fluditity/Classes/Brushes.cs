using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace Fluid.Controls
{
    public static class Brushes
    {
        private static HybridDictionary brushes = new HybridDictionary();

        public static SolidBrush GetBrush(Color color)
        {
            if (brushes.Contains(color)) return brushes[color] as SolidBrush;

            SolidBrush brush = new SolidBrush(color);
            brushes.Add(color, brush);
            return brush;
        }
    }
}
