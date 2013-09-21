using System;

using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Fluid.Controls.Classes
{
    public class PageCollection:BindingList<PageControl>
    {
        public PageCollection(NavigationPanel tabPanel)
            : base()
        {
            this.tabPanel = tabPanel;
        }

        private NavigationPanel tabPanel;

        protected override void OnListChanged(ListChangedEventArgs e)
        {
            base.OnListChanged(e);
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    tabPanel.Controls.Add(this[e.NewIndex]);
                    break;

                case ListChangedType.ItemDeleted:
                    tabPanel.Controls.RemoveAt(e.OldIndex);
                    break;
            }
        }

        protected override void ClearItems()
        {
            tabPanel.Controls.Clear();
            base.ClearItems();
        }
    }
}
