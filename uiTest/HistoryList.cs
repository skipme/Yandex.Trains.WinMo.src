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
    public class HistoryList : FluidListBox
    {
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


            SelectHistoryTemplate template = new SelectHistoryTemplate();
            template.OnClickRemove += new SelectHistoryTemplate.RemoveItem(template_OnClickRemove);
            ItemTemplate = template;
        }
        public delegate void RemoveItem(HistoryItem hi);
        public event RemoveItem OnClickRemove;
        void template_OnClickRemove(HistoryItem hi)
        {
            if (OnClickRemove != null)
                OnClickRemove(hi);
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

        public delegate void HistorySelected(HistoryItem esr);
        public event HistorySelected OnHistorySelected;

        public virtual bool NavigateForward()
        {
            if (OnHistorySelected != null) OnHistorySelected(((List<HistoryItem>)DataSource)[SelectedItemIndex]);
            return false;
        }

        protected override void OnItemClick(int index)
        {
            base.OnItemClick(index);
            NavigateForward();
        }

        public void Populate()
        {
            DataSource = data.HistorySlots.AllSorted();
        }
    }
    public class SelectHistoryTemplate : FluidTemplate
    {
        private FluidLabel titleLabel;
        private FluidLabel cityLabel;
        private FluidButton delButton;

        protected override void InitControl()
        {
            base.InitControl();
            int w0 = Form1.GlobalBounds.Width;
            int h0 = Form1.GlobalBounds.Height / 8;

            int rc = (int)(h0 * .65);

            int bw1 = rc;
            int bw2 = rc;

            this.Bounds = new Rectangle(0, 0, w0, h0);
            titleLabel = new FluidLabel("", bw1 + 4, 0, w0 - bw1 - bw2 - 8, h0);
            titleLabel.LineAlignment = StringAlignment.Near;
            titleLabel.Font = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);
            titleLabel.ForeColor = Color.Empty;

            cityLabel = new FluidLabel("", bw1 + 24, 0, w0 - 8, h0);
            cityLabel.LineAlignment = StringAlignment.Far;
            cityLabel.Font = new Font(FontFamily.GenericSansSerif, 8f, FontStyle.Regular);
            cityLabel.ForeColor = Theme.Theme.Current.GrayTextColor;


            delButton = new FluidButton("-", 2, (h0 - rc) / 2, rc, rc);
            delButton.BackColor = Color.FromArgb(40, 90, 110);
            delButton.Shape = ButtonShape.Rounded;
            delButton.ForeColor = Color.White;
            delButton.Click += new EventHandler(delButton_Click);

            Controls.Add(titleLabel);
            Controls.Add(cityLabel);
            Controls.Add(delButton);

            Font btnFont = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);

            titleLabel.Anchor = AnchorAll;
            cityLabel.Anchor = AnchorAll;
        }

        void delButton_Click(object sender, EventArgs e)
        {
            MessageDialog dialog = new MessageDialog();

            dialog.Message = ((HistoryItem)this.Item).Start.Title + " " + ((HistoryItem)this.Item).End.Title + ". Удалить?";
            dialog.OkText = "Да";
            dialog.CancelText = "Нет";
            dialog.Result += new EventHandler<Fluid.Classes.DialogEventArgs>(dialog_Result);
            dialog.ShowModal(ShowTransition.FromBottom);
        }
        public delegate void RemoveItem(HistoryItem hi);
        public event RemoveItem OnClickRemove;
        void dialog_Result(object sender, Fluid.Classes.DialogEventArgs e)
        {
            if (e.Result == System.Windows.Forms.DialogResult.OK)
                if (OnClickRemove != null)
                    OnClickRemove((HistoryItem)this.Item);
        }

        public void InvalidateItem()
        {
        }


        protected override void OnItemUpdate(object value)
        {
            base.OnItemUpdate(value);
            HistoryItem item = value as HistoryItem;
            if (item != null)
            {
                titleLabel.Text = item.Start.Title + " " + item.End.Title;
                cityLabel.Text = string.Format("{0}, {1}", item.FuzzyTime, item.tip == null ? "" : item.tip.TrainTypeRu);
            }
            else
            {
                titleLabel.Text = "";
            }
        }
    }
}
