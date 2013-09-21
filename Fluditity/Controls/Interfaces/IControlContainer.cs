using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Fluid.Controls
{

    /// <summary>
    /// Interface for a control that hosts one ore more child controls. This is the base class for ISingleControlContainer and IMultiControlContainer.
    /// Note that a IFluidContainer is not necassarily a FluidControl. FluidHost is a windows control and implements
    /// also IFluidContainer.
    /// </summary>
    public interface IContainer
    {
        /// <summary>
        /// Invalidates the canvas of the  specified bounds.
        /// </summary>
        /// <param name="bounds">The bound to invalidate.</param>
        void Invalidate(Rectangle bounds);

        Rectangle Bounds {get;}

        Font Font { get; }

        Color BackColor { get; }

        Color ForeColor { get; }

        /// <summary>
        /// Gets the host for all nested FluidControls.
        /// </summary>
        IHost Host { get; }

        SizeF ScaleFactor { get; }

        Point PointToHost(int x, int y);

        //void Layout(LayoutEventArgs e);

        /// <summary>
        /// Gets the bounds as they appear in the screen.
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        Rectangle GetScreenBounds(Rectangle bounds);

    }
}
