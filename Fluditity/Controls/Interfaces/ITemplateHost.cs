using System;
using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public interface ITemplateHost
    {
        void Bind(FluidTemplate template);

        void Bind(FluidTemplate template, int itemIndex);
    }
}
