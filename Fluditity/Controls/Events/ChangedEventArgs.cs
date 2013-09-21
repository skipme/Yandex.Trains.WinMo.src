using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class ChangedEventArgs<T> : EventArgs
    {
        public ChangedEventArgs()
            : base()
        {
        }

        public ChangedEventArgs(T old, T newValue)
            : base()
        {
            OldValue = old;
            NewValue = newValue;
        }

        public T OldValue { get; internal set; }
        public T NewValue { get; internal set; }
    }
}
