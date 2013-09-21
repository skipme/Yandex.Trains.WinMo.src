using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Fluid.Controls
{
    public interface IHost:IContainer
    {
        void AddControl(Control c);

        void RemoveControl(Control c);

        /// <summary>
        /// Gets or sets the selected control.
        /// </summary>
        FluidControl FocusedControl { get; set; }

        void Update();

        System.Drawing.Graphics CreateGraphics();

        Cursor Cursor { get; set; }
    }
}
