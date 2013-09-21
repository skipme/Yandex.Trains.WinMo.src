using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Fluid.Controls;
using System.Drawing;
using System.Data.SQLite;
using System.Data.Common;

namespace uiTest
{
    public class DirectionList : FluidListBox
    {
        int itemsOnDisplay = 10;
        protected override void InitControl()
        {
            //ItemHeight = 32;
            ItemHeight = Form1.GlobalBounds.Height / itemsOnDisplay;
            base.InitControl();

            BackColor = theme.ListBackColor;
            ForeColor = theme.ListForeColor;
            BorderColor = theme.ListBorderColor;
            ScrollBarButtonColor = theme.ScrollbarColor;
            ScrollBarButtonBorderColor = theme.ScrollbarBorderColor;

            SelectDirectionTemplate template = new SelectDirectionTemplate();
            ItemTemplate = template;
        }

        Theme.Theme theme = uiTest.Theme.Theme.Current;

        protected override void OnPaintItem(ListBoxItemPaintEventArgs e)
        {
            if (e.IsSelected)
            {
                e.BackColor = theme.ListSelectedBackColor;
                e.ForeColor = theme.ListSelectedForeColor;
            }
            else
            {
                e.BackColor = theme.ListBackColor;
                e.ForeColor = theme.ListForeColor;
            }

            if (e.Item is GroupHeader)
            {
                e.BackColor = theme.ListHeaderBackColor;
                e.PaintDefault();
                e.Handled = true;
            }
            else if (e.IsSelected)
            {
                e.BorderColor = e.BorderColor;
                e.PaintHeaderBackground();
                e.PaintDefaultBorder();
                e.BackColor = Color.Transparent;
                e.PaintTemplate();
                e.Handled = true;
            }
            base.OnPaintItem(e);
        }

        public virtual void AddItem()
        {
        }

        public override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                switch (e.KeyCode)
                {
                    case System.Windows.Forms.Keys.Right:
                        e.Handled = NavigateForward();
                        break;

                }
            }
        }

        public delegate void DirectionSelected(string dir);
        public event DirectionSelected OnDirectionSelected;

        public virtual bool NavigateForward()
        {
            if (OnDirectionSelected != null) OnDirectionSelected(((List<string>)DataSource)[SelectedItemIndex]);
            return false;
        }

        public void Populate()
        {
            List<string> directions = SuburbanContext.availableDirections();
            
            if (directions != null)
                DataSource = directions;
            else
            {
                if (SuburbanContext.NetworkNA)
                {
                    MessageDialog.Show("Сеть недоступна", "OK", null);
                }
                else
                {
                    MessageDialog.Show("Произошла ошибка", "OK", null);
                }
            }
        }

        protected override void OnItemClick(int index)
        {
            base.OnItemClick(index);
            NavigateForward();
        }
    }
    public class SelectDirectionTemplate : FluidTemplate
    {
        private FluidLabel titleLabel;
        //private FluidLabel cityLabel;

        protected override void InitControl()
        {
            base.InitControl();
            int w0 = Form1.GlobalBounds.Width;
            int h0 = Form1.GlobalBounds.Height / 8;
            const int bw1 = 24;
            const int bw2 = 24;
            this.Bounds = new Rectangle(0, 0, w0, h0);
            titleLabel = new FluidLabel("", bw1 + 4, 0, w0 - bw1 - bw2 - 8, h0);
            titleLabel.LineAlignment = StringAlignment.Center;
            titleLabel.Font = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);
            titleLabel.ForeColor = Color.Empty;

            //cityLabel = new FluidLabel("", bw1 + 24, 0, w0 - 8, h0);
            //cityLabel.LineAlignment = StringAlignment.Far;
            //cityLabel.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            //cityLabel.ForeColor = Theme.Theme.Current.GrayTextColor;

            Controls.Add(titleLabel);
            //Controls.Add(cityLabel);

            Font btnFont = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);

            titleLabel.Anchor = AnchorAll;
            //cityLabel.Anchor = AnchorAll;
        }
        public void InvalidateItem()
        {
        }

        protected override void OnItemUpdate(object value)
        {
            base.OnItemUpdate(value);
            string item = value as string;
            if (item != null)
            {
                titleLabel.Text = item;
                //cityLabel.Text = item.Direction;
            }
            else
            {
                titleLabel.Text = "";
            }
        }
    }
}
