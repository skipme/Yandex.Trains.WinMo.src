using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;

namespace uiTest
{
    public class HistoryPanel : FluidPanel
    {
        private FluidHeader header = new FluidHeader();
        private StationsList listBox = new StationsList();

        public delegate void ExecBack();
        public event ExecBack OnNewTrip;

        protected override void InitControl()
        {
            base.InitControl();
            //this.EnableDoubleBuffer = true;

            //Bounds = new Rectangle(0, 0, 240, 300);
            //BackColor = Color.Green;
            Anchor = AnchorAll;
            //const int h = 32;
            //header.Bounds = new Rectangle(0, 0, 240, h);
            //listBox.Bounds = new Rectangle(0, h, 240, 300 - h);
            int h = Form1.GlobalBounds.Height / 8;
            Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            header.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            listBox.Bounds = new Rectangle(0, h, Form1.GlobalBounds.Width, Form1.GlobalBounds.Height - h);


            header.Anchor = AnchorTLR;
            listBox.Anchor = AnchorAll;
            header.BackButton.Shape = ButtonShape.Rounded;
            header.BackButton.Visible = true;
            header.BackButton.Text = "Новый";
            header.Title = "История";
            header.BackButton.Click += new EventHandler(BackButton_Click);

            Controls.Add(header);
            Controls.Add(listBox);
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            if (OnNewTrip != null) OnNewTrip();
        }

        public override void Focus()
        {
            listBox.Focus();
            listBox.Refresh();
        }
    }
}
