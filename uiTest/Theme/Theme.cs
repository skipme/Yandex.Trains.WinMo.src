using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace uiTest.Theme
{
    public class Theme
    {
        public Color ListBackColor { get; set; }
        public Color ListSelectedBackColor { get; set; }
        public Color TabHeaderBackColor { get; set; }
        public Color ListHeaderBackColor { get; set; }
        public Color ListForeColor { get; set; }
        public Color ListSecondaryForeColor { get; set; }
        public Color ListAlternateForeColor { get; set; }
        public Color ListSelectedForeColor { get; set; }

        public Color ListSecondarySelectedForeColor { get; set; }
        public Color ListAlternateSelectedForeColor { get; set; }
        public Color ButtonsBackColor { get; set; }
        public Color ButtonsForeColor { get; set; }
        public bool ButtonsGradianted { get; set; }
        public Color TabHeaderButtonBackColor { get; set; }
        public Color TabHeaderButtonForeColor { get; set; }
        public Color ButtonsButtonBackColor { get; set; }
        public Color ListBorderColor { get; set; }
        public Color ScrollbarColor { get; set; }
        public Color ScrollbarBorderColor { get; set; }
        public Color GrayTextColor { get; set; }
        public Color ItemButtonColor { get; set; }

        public static Theme Current = new Default();
    }
}
