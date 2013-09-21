using System;

using System.Collections.Generic;
using System.Text;

namespace Fluid.Controls
{
    public class TemplateEventArgs : ListBoxItemEventArgs
    {
        public TemplateEventArgs(IContainer control)
            : base()
        {
            this.container = control;
        }

        private IContainer container;

        //public TemplateEventArgs(int index, object item, FluidTemplate template)
        //    : base(index, item)
        //{
        //    Template = template;
        //}

        private FluidTemplate template;

        /// <summary>
        /// Gets or sets the template
        /// </summary>
        public FluidTemplate Template
        {
            get { return template; }
            set
            {
                template = value;
                if (template != null)
                {
                    template.BeginInit();
                    template.Container = container;
                }
            }
        }
    }
}
