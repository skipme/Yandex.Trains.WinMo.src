using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;

namespace uiTest
{
    public class SearchPanel : FluidPanel
    {
        private SearchHeader header = new SearchHeader();
        private StationsList listBox = new StationsList();

        public delegate void ExecBack(StationItem itm);
        public event ExecBack backward;

        protected override void InitControl()
        {
            //base.InitControl();

            //Bounds = new Rectangle(0, 0, 240, 300);
            //Anchor = AnchorAll;
            //const int h = 32;
            //header.Bounds = new Rectangle(0, 0, 240, h);
            //listBox.Bounds = new Rectangle(0, h, 240, 300 - h);
            //header.Anchor = AnchorTLR;
            //listBox.Anchor = AnchorAll;
            //header.BackButton.Shape = ButtonShape.Rounded;
            //header.BackButton.Visible = true;
            //header.BackButton.Text = "OK";

            base.InitControl();
            Anchor = AnchorAll;
            int h = Form1.GlobalBounds.Height / 8;
            header.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            listBox.Bounds = new Rectangle(0, h, Form1.GlobalBounds.Width, Form1.GlobalBounds.Height - h);
            header.Anchor = AnchorTLR;
            header.titleLabel.Bounds = new Rectangle((int)(Form1.GlobalBounds.Width * .27), (int)(h * 0.15), (int)(Form1.GlobalBounds.Width * .7), (int)(h * 0.65));
            listBox.Anchor = AnchorAll;
            header.BackButton.Shape = ButtonShape.Rounded;
            header.BackButton.Visible = true;
            header.BackButton.Text = "OK";

            header.Title = "";
            header.OnTextChanged += new SearchHeader.TextChange(header_OnTextChanged);
            header.BackButton.Click += new EventHandler(BackButton_Click);

            listBox.OnStationChanged += new StationsList.StationChanged(listBox_OnStationChanged);

            Controls.Add(header);
            Controls.Add(listBox);
            listBox.DataSource = new List<string>();
        }
        StationItem selected = null;
        void listBox_OnStationChanged(StationItem esr)
        {
            selected = esr;
        }

        void header_OnTextChanged(string expression)
        {
            if (expression.Length >= 2)
            {
                selected = null;
                listBox.PopulateInSearch(expression);
            }
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            if (backward != null && selected != null)
                backward(selected);
        }

        public override void Focus()
        {
            listBox.Focus();
            listBox.Refresh();
        }
    }
}
