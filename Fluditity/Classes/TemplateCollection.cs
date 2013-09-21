using System;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Fluid.Controls
{
    public class TemplateCollection : BindingList<FluidTemplate>
    {
        public TemplateCollection(FluidListBox owner)
            : base()
        {
            this.owner = owner;
        }

        private FluidListBox owner;

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    FluidTemplate t = this[e.NewIndex];
                    t.BeginInit();
                    t.Container = owner;
                    t.Scale(owner.ScaleFactor);
                    t.Bounds = owner.GetItemBounds(0);
                    t.EndInit();
                    break;

            }
        }

        protected override void RemoveItem(int index)
        {
            this[index].Container = null;
            base.RemoveItem(index);
        }
    }
}
