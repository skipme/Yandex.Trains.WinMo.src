using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Fluid.Controls.Classes;
using Fluid.Drawing;
using System.Windows.Forms;
using Fluid.Drawing.GdiPlus;
using Fluid.Classes;

namespace Fluid.Controls
{
    public class FluidHeader : ControlContainer, ILayoutPanel
    {
        protected override void InitControl()
        {
            base.InitControl();
            backButton = defaultBackButton;
            Anchor = AnchorTLR;
            if (Bounds.Size.IsEmpty) bounds = new Rectangle(0, 0, Globals.bounds.Width, Globals.bounds.Height / 10);
            ForeColor = Color.White;
            //       Bounds = new Rectangle(0, 0, 240, 28);
            rightButtons.Font = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);
            Font = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);
            BackColor = Color.FromArgb(59, 109, 133);// Color.SlateGray;
            int h = (int)(Height * .8);
            int wfracb = (int)(Globals.bounds.Width * .2);

            backButton.Visible = false;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);
            backButton.Bounds = new Rectangle(2, 4, wfracb, h);
            backButton.Shape = ButtonShape.Back;
            backButton.Anchor = AnchorLTB;
            backButton.BackColor = Color.FromArgb(40, 90, 110);//ColorConverter.OpaqueColor(Color.SlateGray);

            rightButtons.Bounds = new Rectangle(Width - wfracb - 4, 4, wfracb, h);
            rightButtons.Anchor = AnchorRTB;
            titleLabel.Anchor |= AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            titleLabel.ForeColor = Color.Empty;
            titleLabel.ShadowColor = Color.Black;
            titleLabel.LineAlignment = StringAlignment.Center;
            titleLabel.Alignment = StringAlignment.Center;
            rightButtons.ButtonsChanged += new EventHandler(rightButtons_ButtonsChanged);
            rightButtons.Visible = false;
            //    InitTitleBounds();
            titleLabel.bounds = ClientRectangle;

            controls.Add(titleLabel);
            controls.Add(backButton);
            controls.Add(rightButtons);
        }


        public override void Dispose()
        {
            titleLabel.Dispose();
            backButton.Dispose();
            if (animLabel != null) animLabel.Dispose();
            base.Dispose();
        }

        protected override void OnForeColorChanged()
        {
            base.OnForeColorChanged();
            //    titleLabel.Text = ForeColor;
        }

        void rightButtons_ButtonsChanged(object sender, EventArgs e)
        {
            rightButtons.Visible = rightButtons.Buttons.Count > 0;
        }


        private int animOffset = 0;
        internal int AnimOffset
        {
            get { return animOffset; }
            set
            {
                if (animOffset != value)
                {
                    animOffset = value;
                    InitTitleBounds();
                    Invalidate(titleLabel.Bounds);
                    Invalidate(animLabel.Bounds);
                }
            }
        }

        private FluidButton animBackButton;
        private ButtonGroup animButtons;
        private FluidLabel animLabel;

        internal FluidLabel AnimLabel
        {
            get
            {
                EnsureAnimLabel();
                return animLabel;
            }
        }

        internal FluidButton AnimBackButton
        {
            get { return animBackButton; }
            set
            {
                if (animBackButton != value)
                {
                    animBackButton = value;
                    if (value != null)
                    {
                        if (animBackButton != null) controls.Remove(animBackButton);
                        animBackButton.Bounds = backButton.Bounds;
                        controls.Add(value);
                    }
                }
            }
        }

        internal ButtonGroup AnimButtons
        {
            get { return animButtons; }
            set
            {
                if (animButtons == null)
                {
                    animButtons = value;
                    if (value != null)
                    {
                        controls.Add(animButtons);
                        animButtons.Font = rightButtons.Font;
                        animButtons.Bounds = rightButtons.Bounds;
                    }
                }
            }
        }

        private void EnsureAnimLabel()
        {
            if (animLabel == null)
            {
                FluidLabel l = new FluidLabel("", Width, 0, Width, Height);
                animLabel = l;
                l.ShadowColor = Color.Black;
                l.LineAlignment = StringAlignment.Center;
                l.Alignment = StringAlignment.Center;
                l.Visible = false;
                controls.Insert(0, l);
            }
        }

        private void InitTitleBounds()
        {
            int w = Width;
            int h = Height - 8;
            int l = backButton.Visible ? 0 : backButton.Right;
            int r = w - (rightButtons.Visible ? w : rightButtons.Left);
            l = Math.Min(l, r);
            r = w + l;


            w = Width - l - l;
            int a = (animOffset * w / Width);

            titleLabel.Bounds = new Rectangle(l - a, 4, w, h);
            AnimLabel.Bounds = new Rectangle(l + w - a, 4, w, h);
        }

        public void SetDefaultBackButton()
        {
            BackButton = defaultBackButton;
        }




        public ButtonGroup rightButtons = new ButtonGroup();
        private FluidButton defaultBackButton = new FluidButton();
        private FluidLabel titleLabel = new FluidLabel();


        public FluidButton backButton;

        public bool IsDefaultBackButton { get { return backButton == defaultBackButton; } }

        public FluidButton BackButton
        {
            get { return backButton; }
            set
            {
                if (backButton != value)
                {
                    if (backButton != null)
                    {
                        controls.Remove(backButton);
                    }
                    backButton = value;
                    if (backButton != null)
                    {
                        if (backButton.Font == null)
                        {
                            backButton.Font = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);
                        }
                        if (backButton.BackColor.IsEmpty)
                        {
                            backButton.BackColor = defaultBackButton.BackColor;
                        }
                        int h = backButton.Height;//;UnscaleX(Height) - 8;
                        int w = backButton.Width;// Math.Min(backButton.Width, 32);
                        backButton.Bounds = new Rectangle(2, 4, w, h);
                        backButton.Anchor = backButton.Anchor;//AnchorLTB;
                        controls.Insert(2, backButton);
                    }
                    Invalidate();
                }
            }
        }

        public string Title
        {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        public ButtonCollection Buttons
        {
            get { return rightButtons.Buttons; }
        }

        public int ButtonsWidth
        {
            get { return rightButtons.Width; }
            set { rightButtons.Width = value; }
        }


        public RoundedCorners Corners
        {
            get { return rightButtons.Corners; }
            set { rightButtons.Corners = value; }
        }

        public ButtonShape RightShape
        {
            get { return backButton.Shape; }
            set { backButton.Shape = value; }
        }

        protected override void OnPaintBackground(FluidPaintEventArgs e)
        {
            PaintGradientBackground(e);
        }

        protected override void OnSizeChanged(Size oldSize, Size newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            InitTitleBounds();
        }


        internal int ButtonsAlpha { set { rightButtons.Alpha = value; } }
    }
}
