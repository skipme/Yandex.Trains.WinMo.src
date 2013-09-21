using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;

namespace uiTest
{
    public class CitySearchPanel : FluidPanel
    {
        private SearchHeader header = new SearchHeader();
        private CityList listBox = new CityList();

        public delegate void ExecBack(int cityid);
        public event ExecBack OnOkay;

        protected override void InitControl()
        {
            base.InitControl();
            //this.EnableDoubleBuffer = true;
           
            //Bounds = new Rectangle(0, 0, 240, 300);
            //BackColor = Color.Green;
            Anchor = AnchorAll;
            //const int h = 32;
            int h = Form1.GlobalBounds.Height / 8;
            //Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            header.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            listBox.Bounds = new Rectangle(0, h, Form1.GlobalBounds.Width, Form1.GlobalBounds.Height - h);

            //header.Bounds = new Rectangle(0, 0, Form1.GlobalBounds.Width, h);
            //listBox.Bounds = new Rectangle(0, h, Form1.GlobalBounds.Width, this.Height - h);

            header.Anchor = AnchorTLR;
            header.titleLabel.Bounds = new Rectangle((int)(Form1.GlobalBounds.Width * .27), (int)(h * 0.15), (int)(Form1.GlobalBounds.Width * .7), (int)(h * 0.65));
            listBox.Anchor = AnchorAll;
            header.BackButton.Shape = ButtonShape.Rounded;
            header.BackButton.Visible = true;
            header.BackButton.Text = "OK";
            //header.next
            header.Title = "";
            header.OnTextChanged += new SearchHeader.TextChange(header_OnTextChanged);
            header.BackButton.Click += new EventHandler(BackButton_Click);

            Controls.Add(header);
            Controls.Add(listBox);

            listBox.OnCitySelected += new CityList.CitySelected(listBox_OnCitySelected);

            CityItem cself = SuburbanContext.SearchCity(-1);
            listBox.DataSource = new List<CityItem>() { cself };
            nowcityid = cself.ID;
        }
        
        int nowcityid = -1;
        void listBox_OnCitySelected(CityItem city)
        {
            nowcityid = city.ID;
        }

        void header_OnTextChanged(string expression)
        {
            if (expression.Length >= 2)
                listBox.PopulateInSearch(expression);
        }

        void BackButton_Click(object sender, EventArgs e)
        {
            if (OnOkay != null && nowcityid > 0)
                OnOkay(nowcityid);
        }

        public override void Focus()
        {
            listBox.Focus();
            listBox.Refresh();
        }
    }
}
