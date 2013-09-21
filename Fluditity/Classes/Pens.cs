using System;

using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Drawing;

namespace Fluid.Controls
{
    public static class Pens
    {
        private static HybridDictionary pens = new HybridDictionary();

        public static Pen GetPen(Color color)
        {
            if (pens.Contains(color)) return pens[color] as Pen;

            Pen pen = new Pen(color);
            pens.Add(color, pen);
            return pen;
        }
    }
}
