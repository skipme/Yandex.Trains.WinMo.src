using System;

using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Fluid.Controls.Classes;
using Fluid.Drawing;
using System.Windows.Forms;
using Fluid.Drawing.GdiPlus;
using Fluid.Controls;

namespace Fluid.Controls
{

    public class SearchHeader : ControlContainer, ILayoutPanel
    {
        protected override void InitControl()
        {
            base.InitControl();
            backButton = defaultBackButton;
            Anchor = AnchorTLR;
            if (Bounds.Size.IsEmpty)
                //bounds = new Rectangle(0, 0, 240, 32);
                bounds = new Rectangle(0, 0, Fluid.Classes.Globals.bounds.Width, Fluid.Classes.Globals.bounds.Height / 8);
            ForeColor = Color.White;
            //       Bounds = new Rectangle(0, 0, 240, 28);
            rightButtons.Font = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);
            Font = new Font(FontFamily.GenericSansSerif, 10f, FontStyle.Bold);
            BackColor = Color.SlateGray;
            int h = Height - 8;

            backButton.Visible = false;
            backButton.Font = new Font(FontFamily.GenericSansSerif, 6f, FontStyle.Regular);
            backButton.Bounds = new Rectangle(2, 4, 48, h);
            backButton.Shape = ButtonShape.Back;
            backButton.Anchor = AnchorLTB;
            backButton.BackColor = ColorConverter.OpaqueColor(Color.SlateGray);

            //rightButtons.Bounds = new Rectangle(Width - 60 - 4, 8, 60, h);
            //rightButtons.Anchor = AnchorRTB;
            titleLabel = new FluidTextBox("", 50, (int)(h * 0.35), (int)(h * 0.65), h);
            titleLabel.Anchor |= AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            titleLabel.ForeColor = Color.Black;
            //titleLabel.Anchor = AnchorLTB;
            //titleLabel.Bounds = new Rectangle(50, (int)(h * 0.35), (int)(h * 0.65), h);
            //titleLabel.ShadowColor = Color.Black;
            //titleLabel.LineAlignment = StringAlignment.Center;
            //titleLabel.Alignment = StringAlignment.Center;
            //rightButtons.ButtonsChanged += new EventHandler(rightButtons_ButtonsChanged);
            //rightButtons.Visible = false;

            titleLabel.KeyUp += new EventHandler(titleLabel_KeyUp);
            InitTitleBounds();
            //titleLabel.bounds = ClientRectangle;

            controls.Add(titleLabel);
            controls.Add(backButton);
            //controls.Add(rightButtons);
        }

        public override void OnEnter(IHost host)
        {
            base.OnEnter(host);
        }

        void titleLabel_KeyUp(object sender, EventArgs e)
        {
            if (OnTextChanged != null)
                OnTextChanged(titleLabel.Text);
        }

        public delegate void TextChange(string expression);
        public event TextChange OnTextChanged;


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
        public FluidTextBox titleLabel;

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
            titleLabel.Bounds = new Rectangle(52, 4, 110, 24);
            return;
            //int w = Width;
            //int h = Height - 8;
            //int l = backButton.Visible ? 0 : backButton.Right;
            //int r = w - (rightButtons.Visible ? w : rightButtons.Left);
            //l = Math.Min(l, r);
            //r = w + l;


            //w = Width - l - l;
            //int a = (animOffset * w / Width);

            //titleLabel.Bounds = new Rectangle(l - a, 4, w, h);
            //AnimLabel.Bounds = new Rectangle(l + w - a, 4, w, h);
        }

        public void SetDefaultBackButton()
        {
            BackButton = defaultBackButton;
        }




        private ButtonGroup rightButtons = new ButtonGroup();
        private FluidButton defaultBackButton = new FluidButton();
        //private FluidLabel titleLabel = new FluidLabel();


        private FluidButton backButton;

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
                        int h = UnscaleX(Height) - 8;
                        int w = Math.Min(backButton.Width, 32);
                        backButton.Bounds = new Rectangle(2, 4, w, h);
                        backButton.Anchor = AnchorLTB;
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
