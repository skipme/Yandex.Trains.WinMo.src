using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;

namespace uiTest
{
    public class TestPanel : FluidPanel
    {
        private FluidHeader header = new FluidHeader();
        private StationsList listBox = new StationsList();

        public delegate void ExecBack();
        public event ExecBack backward;

        protected override void InitControl()
        {
            base.InitControl();
            //this.EnableDoubleBuffer = true;

            Bounds = new Rectangle(0, 0, 240, 300);
            //BackColor = Color.Green;
            Anchor = AnchorAll;
            const int h = 32;
            header.Bounds = new Rectangle(0, 0, 240, h);
            listBox.Bounds = new Rectangle(0, h, 240, 300 - h);
            header.Anchor = AnchorTLR;
            listBox.Anchor = AnchorAll;
            header.BackButton.Shape = ButtonShape.Rounded;
            header.BackButton.Visible = true;
            header.BackButton.Text = "OK";
            header.Title = "Список";
            header.BackButton.Click += new EventHandler(BackButton_Click);

            Controls.Add(header);
            Controls.Add(listBox);
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            backward();
            return;
        }

        public override void Focus()
        {
            listBox.Focus();
            listBox.Refresh();
        }
    }
}
