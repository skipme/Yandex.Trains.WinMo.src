using System;
using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public interface ICommandContainer
    {
        void RaiseCommand(CommandEventArgs e);

        event EventHandler<CommandEventArgs> Command;
    }
}
