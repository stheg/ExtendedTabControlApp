using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ExtendedTabControlApp
{
    public class TabItemFactory
    {
        /// <summary>
        /// Create instance of TabItem
        /// </summary>
        /// <param name="isSelectedField">Sets IsSelected value in the created TabItem</param>
        /// <returns></returns>
        public virtual TabItem CreateTabItem(bool isSelectedField = false)
        {
            return new TabItem() { IsSelected = isSelectedField };
        }
    }

    public class ClosableTabItemFactory : TabItemFactory
    {
        public override TabItem CreateTabItem(bool isSelectedField = false)
        {
            return new ClosableTabItem() { IsSelected = isSelectedField };
        }
    }
}
