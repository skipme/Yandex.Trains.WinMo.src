using System;

using System.Collections.Generic;
using System.Text;
using Fluid.Controls.Classes;
using System.Drawing;
using Fluid.Drawing;

namespace Fluid.Controls
{
    public class ButtonGroup : ControlContainer, ICommandContainer,ILayoutPanel
    {
        public ButtonGroup()
            : base()
        {
        }

        public ButtonGroup(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
        }

        protected override void InitControl()
        {
            buttons = new ButtonCollection(this);
            base.InitControl();
        }


        private RoundedCorners corners = RoundedCorners.All;
        public RoundedCorners Corners
        {
            get { return corners; }
            set
            {
                if (corners != value)
                {
                    corners = value;
                    Render();
                }
            }
        }


        private ButtonCollection buttons;

        public ButtonCollection Buttons
        {
            get { return buttons; }
        }

        public void Add(FluidButton button)
        {
            buttons.Add(button);
        }

        public void RemoveAt(int index)
        {
            buttons.RemoveAt(index);
        }

        public void Clear()
        {
            buttons.Clear();
        }

        private int buttonWidth = 0;

        /// <summary>
        /// Gets or sets the width for all but the last button. if set to 0, the width is determined automatically.
        /// </summary>
        public int ButtonWidth
        {
            get { return buttonWidth; }
            set
            {
                if (buttonWidth != value)
                {
                    buttonWidth = value;
                    Render();
                    Invalidate();
                }
            }
        }

        protected override void OnPaintBackground(FluidPaintEventArgs e)
        {
            // do nothing!
        }

        /// <summary>
        /// Renders the buttons;
        /// </summary>
        public void Render()
        {
            if (buttons == null) return;
            int c = buttons.Count;
            controls.Clear();
            int index = 0;
            int x = 0;
            int y = 0;
            int h = Bounds.Height;
            int w0 = c > 0 ? Bounds.Width / c : 0;

            if (buttonWidth > 0) w0 = Math.Min(buttonWidth, Width);

            c--;
            RoundedCorners corners = c == 0 ? RoundedCorners.All : RoundedCorners.Left;
            foreach (FluidButton btn in buttons)
            {
                btn.BeginInit();
                int w = (index != c) ? w0 : (Bounds.Width - x);
                if (btn.BackColor == Color.Transparent || btn.BackColor.IsEmpty)
                {
                    btn.BackColor = this.BackColor;
                }
                btn.Bounds = new Rectangle(x, y, w + ((index != c) ? (int)ScaleFactor.Width : 0), h);
                btn.ScaleFactor = ScaleFactor;
                btn.Corners = corners & Corners;
                btn.Command = index.ToString();
                //btn.Alpha = alpha;
                btn.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom;
                btn.EndInit();
                index++;
                controls.Add(btn);
                x += w;
                corners = index == c ? RoundedCorners.Right : RoundedCorners.None;
            }
            Invalidate();
            OnButtonsChanged();
        }

        private int alpha = 255;
        public int Alpha
        {
            get { return alpha; }
            set
            {
                if (alpha != value)
                {
                    alpha = value;
                    ModifyAlpha(alpha);
                }
            }
        }

        private void ModifyAlpha(int alpha)
        {
            foreach (FluidControl c in controls)
            {
                FluidButton btn = (FluidButton)c;
                btn.Alpha = alpha;
            }
        }

        protected virtual void OnButtonsChanged()
        {
            if (ButtonsChanged != null) ButtonsChanged(this, EventArgs.Empty);
        }

        public event EventHandler ButtonsChanged;


        private ButtonGroupEventArgs clickEvent;
        public override void RaiseCommand(CommandEventArgs e)
        {
            // base.RaiseCommand(e); // do not fire to parent containers!
            if (ButtonClick != null)
            {
                if (clickEvent == null) clickEvent = new ButtonGroupEventArgs();
                int index = int.Parse(e.Command);
                clickEvent.Index = index;
                clickEvent.Button = buttons[index];
                ButtonClick(this, clickEvent);
            }

        }

        protected override void OnSizeChanged(Size oldSize, Size newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            if (!oldSize.IsEmpty)
            {
                Render();
            }
        }

        /// <summary>
        /// Occurs when a button was clicked.
        /// </summary>
        public event EventHandler<ButtonGroupEventArgs> ButtonClick;

        public override void Scale(SizeF scaleFactor)
        {
            base.Scale(scaleFactor);
            ButtonWidth = (int)(ButtonWidth * scaleFactor.Width);
        }
    }
}
