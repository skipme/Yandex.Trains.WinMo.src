using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Fluid.Drawing.GdiPlus;

namespace uiTest.Theme
{
    public class Default : Theme
    {
        public Default()
        {
            this.ListBackColor = Color.White;
            this.ListForeColor = Color.Black;
            this.TabHeaderBackColor = Color.WhiteSmoke;
            this.ListHeaderBackColor = Color.BlanchedAlmond;
            this.ListSelectedBackColor = Color.FromArgb(99, 158, 181);
            this.ListSelectedForeColor = Color.Black;
            this.ButtonsBackColor = Color.SlateGray;
            this.ButtonsForeColor = Color.White;
            this.ButtonsGradianted = true;
            this.TabHeaderButtonBackColor = ColorConverter.AlphaBlendColor(Color.Black, Color.SlateGray, 200);
            this.ButtonsButtonBackColor = ColorConverter.AlphaBlendColor(Color.Black, Color.SlateGray, 200);
            this.TabHeaderButtonForeColor = Color.White;
            this.ListBorderColor = Color.FromArgb(199, 227, 239);
            this.ScrollbarBorderColor = Color.Silver;
            this.ScrollbarColor = Color.Black;
            this.ListSecondaryForeColor = Color.SteelBlue;
            this.ListSecondarySelectedForeColor = ColorConverter.AlphaBlendColor(Color.Black, Color.DarkOrange, 160);
            this.GrayTextColor = Color.Gray;
            ItemButtonColor = Color.SlateGray;


            // this.ListHeaderBackColor = Color.Red;

            this.ButtonsBackColor = Color.Black;
            this.ButtonsButtonBackColor = Color.Black;
            //this.ListBackColor = Color.Black;
            //this.ListForeColor = Color.White;
            //this.ListBorderColor = Color.Gray;

        }
    }
}
