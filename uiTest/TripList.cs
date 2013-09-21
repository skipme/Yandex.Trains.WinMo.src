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
    public class TripList : FluidListBox
    {
        public int SetIndexToTop = -1;
        int itemsOnDisplay = 9;
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

            TripItemTemplate template = new TripItemTemplate();
            ItemTemplate = template;

            DataSource = new List<TripItem>()
            {
                  new TripItem()
                 {
                     Title = "",
                     Arrival = DateTime.Now,
                     Departure = DateTime.Now,
                     Duration = ""
                 }
            };

        }

        Theme.Theme theme = uiTest.Theme.Theme.Current;

        protected override void OnPaintItem(ListBoxItemPaintEventArgs e)
        {
            bool background_grad = false;
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
            if (e.ItemIndex == SetIndexToTop)
            {
                e.BackColor = Color.FromArgb(148, 205, 148);
                e.ForeColor = Color.FromArgb(252, 252, 252);
                background_grad = true;
            }

            if (((TripItem)e.Item).Express)
            {
                e.BackColor = Color.FromArgb(205, 148, 148);
                e.ForeColor = Color.FromArgb(252, 252, 252);
                background_grad = true;
            }

            if (e.Item is GroupHeader)
            {
                e.BackColor = theme.ListHeaderBackColor;
                e.PaintDefault();
                e.Handled = true;
            }
            else if (e.IsSelected || background_grad)
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
        public override void OnEnter(IHost host)
        {
            base.OnEnter(host);
            if (SetIndexToTop > 0)
            {
                //SelectItem(((System.Collections.IList)DataSource).Count - 1);
                SelectItem(SetIndexToTop);
            }
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

        public delegate void TripSelected(TripItem trip);
        public event TripSelected OnTripSelected;

        public virtual bool NavigateForward()
        {
            if (OnTripSelected != null)
            {
                TripItem li = ((List<TripItem>)DataSource)[SelectedItemIndex];
                OnTripSelected(li);
                string template = "Поезд №{0}: {1}; Тариф: {2}\r\nДни: {3}\r\nОстановки: {4}";
                MessageDialog.Show(string.Format(template, li.Number, li.TrainTypeRu, li.ClarifiedTariff, li.Days, li.Stops), "OK", null);
            }
            return false;
        }

        protected override void OnItemClick(int index)
        {
            base.OnItemClick(index);
            NavigateForward();
        }
        public void PopulateWithSearch()
        {
            PopulateWithSearch(false);
        }
        public void PopulateWithSearch(bool sync)
        {
            List<TripItem> dsl = new List<TripItem>();

            dsl = SuburbanContext.FindTrips(DateTime.Now, sync);
            if (dsl != null)
            {
                DataSource = dsl;
                DateTime now = DateTime.Now;
                SetIndexToTop = -1;
                for (int i = 0; i < dsl.Count; i++)
                {
                    if (/*dsl[i].Arrival > now ||*/ dsl[i].Departure > now)
                    {
                        SetIndexToTop = i;
                        if (sync) SelectItem(SetIndexToTop);
                        break;
                    }
                }
            }
        }
    }
    public class TripItemTemplate : FluidTemplate
    {
        private FluidLabel timeLabel;
        private FluidLabel deptimeLabel;
        private FluidLabel titleLabel;
        private FluidLabel cityLabel;

        protected override void InitControl()
        {
            base.InitControl();
            int w0 = Form1.GlobalBounds.Width;
            int h0 = Form1.GlobalBounds.Height / 8;
            const int bw1 = 24;
            const int bw2 = 24;
            this.Bounds = new Rectangle(0, 0, w0, h0);
            timeLabel = new FluidLabel("", 2, 0, (int)(w0 * .16), (h0 / 2) + 3);
            timeLabel.LineAlignment = StringAlignment.Far;
            timeLabel.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Bold);
            timeLabel.ForeColor = Color.Black;

            deptimeLabel = new FluidLabel("", 2, 0, (int)(w0 * .16), h0);
            deptimeLabel.LineAlignment = StringAlignment.Far;
            deptimeLabel.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            deptimeLabel.ForeColor = Theme.Theme.Current.GrayTextColor;

            titleLabel = new FluidLabel("", (int)(w0 * .16), 0, w0 - (int)(w0 * .16), (h0 / 2) + 3);
            titleLabel.LineAlignment = StringAlignment.Far;
            titleLabel.Font = new Font(FontFamily.GenericSansSerif, 7f, FontStyle.Regular);
            titleLabel.ForeColor = Color.Black;

            cityLabel = new FluidLabel("", (int)(w0 * .16), 0, w0 - 8, h0);
            cityLabel.LineAlignment = StringAlignment.Far;
            cityLabel.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            cityLabel.ForeColor = Theme.Theme.Current.GrayTextColor;

            Controls.Add(timeLabel);
            Controls.Add(titleLabel);
            Controls.Add(cityLabel);
            Controls.Add(deptimeLabel);

            Font btnFont = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);

            timeLabel.Anchor = AnchorAll;
            cityLabel.Anchor = AnchorAll;
            titleLabel.Anchor = AnchorAll;
            deptimeLabel.Anchor = AnchorAll;
        }




        public void InvalidateItem()
        {
        }


        protected override void OnItemUpdate(object value)
        {
            base.OnItemUpdate(value);
            TripItem item = value as TripItem;
            if (item != null)
            {
                //if (item.Arrival > item.Departure)
                //{
                //    timeLabel.Text = item.Departure.ToString("HH:mm");
                //    deptimeLabel.Text = item.Arrival.ToString("HH:mm");
                //}
                //else
                //{
                //    TimeSpan span = item.Departure - item.Arrival;
                //    if (span.TotalHours > 5)
                //    {
                //        timeLabel.Text = item.Arrival.ToString("HH:mm");
                //        deptimeLabel.Text = item.Departure.ToString("HH:mm");

                //    }
                //    else
                //    {
                //        timeLabel.Text = item.Departure.ToString("HH:mm");
                //        deptimeLabel.Text = item.Arrival.ToString("HH:mm");
                //    }
                //}
                timeLabel.Text = item.Departure.ToString("HH:mm");
                deptimeLabel.Text = item.Arrival.ToString("HH:mm");
                titleLabel.Text = item.Title;
                cityLabel.Text = string.Format("{1} {2}", item.Duration, item.Days, item.Stops);
            }
            else
            {
                //titleLabel.Text = "";
            }
        }
    }
}
