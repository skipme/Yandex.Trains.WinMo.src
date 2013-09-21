using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace Fluid.Controls
{
#if false
    [Obsolete("Use FluidTextBox insteade.")]
    public class NativeFluidTextBox : FluidLabel
    {
        public NativeFluidTextBox(string text, int x, int y, int width, int height)
            : base(text, x, y, width, height)
        {
            BackColor = Color.White;
            ForeColor = SystemColors.ControlText;
            LineAlignment = StringAlignment.Center;
        }
        public override bool OnClick(PointEventArgs e)
        {
            base.OnClick(e);
            return true;
        }

        public override void OnPaint(FluidPaintEventArgs e)
        {
            base.OnPaint(e);
            if (showBorder)
            {
                using (Pen pen = new Pen(Color.Black))
                {
                    Rectangle bounds = e.ControlBounds;
                    bounds.Width--;
                    bounds.Height--;
                    e.Graphics.DrawRectangle(pen, bounds);
                }
            }
        }

        private TextBox box;

        private bool showBorder = true;

        [DefaultValue(true)]
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                if (showBorder != value)
                {
                    showBorder = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// When the control is entered and get the focus, a textbox window control is added to the 
        /// windows host control.
        /// </summary>
        public override void OnEnter(IHost host)
        {
            if (box == null)
            {
                box = new TextBox();
                box.Font = Font;
                box.BorderStyle = BorderStyle.None;
                box.TextChanged += new EventHandler(box_TextChanged);
                box.KeyDown += new KeyEventHandler(box_KeyDown);
                box.KeyPress += new KeyPressEventHandler(box_KeyPress);
                box.GotFocus += new EventHandler(box_GotFocus);
                box.LostFocus += new EventHandler(box_LostFocus);
                box.BackColor = BackColor;
            }
            Rectangle r = HostBounds;
            r.Inflate((int)(3 * -ScaleFactor.Width), (int)-ScaleFactor.Height);
            box.Text = Text;
            int dy = (r.Height - box.Height) / 2;
            r.Y += dy;
            box.Bounds = r;
            host.AddControl(box);
            box.Focus();
            Invalidate();
            base.OnEnter(host);
        }


        public void SelectAll()
        {
            if (box != null) box.SelectAll();
        }

        public void Select(int start, int length)
        {
            if (box != null) box.Select(start, length);
        }

        void box_LostFocus(object sender, EventArgs e)
        {
            PerformGotFocus();
        }

        void box_GotFocus(object sender, EventArgs e)
        {
            PerformLostFocus();
        }

        void box_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') OnLostFocus();
        }

        void box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                OnLostFocus();
            }
        }

        /// <summary>
        /// When the control leaves the focus, the windows textbox control is removed from the windows host control.
        /// </summary>
        public override void OnLeave(IHost host)
        {
            TextBox b = box;
            box = null;
            if (b != null)
            {
                Text = b.Text;
                b.Visible = false;
                host.RemoveControl(b);
                b.Dispose();
                Invalidate();
            }
            base.OnLeave(host);
        }

        void box_TextChanged(object sender, EventArgs e)
        {
            Text = box.Text;
        }

        public override bool Selectable { get { return true; } }

        protected override void OnTextChanged()
        {
            if (box != null) box.Text = Text;
            if (!Initializing)
            {
                if (TextChanged != null) TextChanged(this, EventArgs.Empty);
            }
            base.OnTextChanged();
        }

        /// <summary>
        /// Occurs when the text has changed.
        /// </summary>
        public event EventHandler TextChanged;


        public override bool Active { get { return true; } }

    }
#endif
}
