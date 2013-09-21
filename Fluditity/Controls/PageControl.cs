using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    /// <summary>
    /// A control that represents a tab item for a tab control.
    /// </summary>
    public class PageControl : ControlContainer
    {
        public PageControl(string title)
            : base()
        {
            this.Title = title;
        }

        protected override void InitControl()
        {
            base.InitControl();
            if (Bounds.IsEmpty) bounds = new System.Drawing.Rectangle(0, 0, 240, 300);
            buttons = new ButtonCollection(null);
            buttons.ListChanged += new System.ComponentModel.ListChangedEventHandler(buttons_ListChanged);
        }

        void buttons_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {

        }

        private FluidControl control;
        //internal NavigationPanel tabPanel;

        /// <summary>
        /// Gets or sets the child control.
        /// </summary>
        public FluidControl Control
        {
            get { return control; }
            set
            {
                if (control != value)
                {
                    if (control != null) controls.Remove(control);
                    control = value;
                    if (value != null)
                    {
                        controls.Add(value);
                        value.Bounds = ClientRectangle;
                    }
                }
            }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set
            {
                if (title != value)
                {
                    title = value;
                    OnTitleChanged();
                }
            }
        }

        private FluidButton backButton;

        public FluidButton BackButton
        {
            get { return backButton; }
            set { backButton = value; }
        }

        private ButtonCollection buttons;

        public ButtonCollection Buttons
        {
            get { return buttons; }
        }

        private void OnTitleChanged()
        {
            if (TitleChanged != null) TitleChanged(this, EventArgs.Empty);
        }

        public event EventHandler TitleChanged;

        protected override void OnSizeChanged(System.Drawing.Size oldSize, System.Drawing.Size newSize)
        {
            base.OnSizeChanged(oldSize, newSize);
            if (control != null) control.Bounds = ClientRectangle;
        }

        protected override void OnPaintBackground(FluidPaintEventArgs e)
        {
            //base.OnPaintBackground(e);
        }

        public override void Scale(System.Drawing.SizeF scaleFactor)
        {
            base.Scale(scaleFactor);
            Buttons.Width = (int)(Buttons.Width * scaleFactor.Width);
        }

        public override void Focus()
        {
            if (Control != null) Control.Focus();
        }

    }
}
