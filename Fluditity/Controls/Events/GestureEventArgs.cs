using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class GestureEventArgs : EventArgs
    {
        public GestureEventArgs(Gesture gesture, bool isPressed, int distance, int ppm)
            : base()
        {
            Gesture = gesture;
            IsPressed = isPressed;
            PixelPerMs = ppm;
            this.Distance = distance;
        }

        internal GestureEventArgs()
            : base()
        {
        }

        /// <summary>
        /// Gets or sets the gesture that occured.
        /// </summary>
        public Gesture Gesture { get; set; }

        /// <summary>
        /// Gets or sets whether the event is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Gets whether the stylus, finger or mouse button is pressed.
        /// </summary>
        public bool IsPressed { get; internal set; }

        /// <summary>
        /// Gets the number of pixels per miliseconds of the gesture.
        /// </summary>
        public int PixelPerMs { get; internal set; }

        public int Distance { get; internal set; }
    }
}
