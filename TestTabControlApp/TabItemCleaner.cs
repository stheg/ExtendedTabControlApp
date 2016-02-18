using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ExtendedTabControlApp
{
    public class TabItemCleaner
    {
        /// <summary>
        /// Remove Content of TabItem from memory
        /// </summary>
        /// <param name="tabItem"></param>
        public virtual void ClearTabItem(TabItem tabItem)
        {
            RecursiveClearPanelsChildren(tabItem.Header as Panel);

            RecursiveClearPanelsChildren(tabItem.Content as Panel);
        }

        /// <summary>
        /// Remove Panel objects from memory
        /// </summary>
        /// <param name="p">Panel which Children which must be removed (for example: TabItem.Content) </param>
        protected virtual void RecursiveClearPanelsChildren(Panel p)
        {
            if (p == null) return;

            foreach (UIElement ch in p.Children)
            {
                if (ch is Panel)
                {
                    RecursiveClearPanelsChildren(ch as Panel);
                }
            }

            p.Children.Clear();

            p = null;
        }
    }

    public class ClosableTabItemCleaner : TabItemCleaner
    {
        public override void ClearTabItem(TabItem tabItem)
        {
            (tabItem.Header as ClosableHeader).txtHeader = null;
            (tabItem.Header as ClosableHeader).btnClose = null;

            base.ClearTabItem(tabItem);
        }
    }
}
