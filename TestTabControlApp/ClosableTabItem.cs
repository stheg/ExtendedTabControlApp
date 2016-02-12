using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtendedTabControlApp
{
    public class ClosableTabItem : TabItem
    {
        private const String TextAddTag = "+";
        private const String TextNewTab = "Новая вкладка";

        public String HeaderText
        {
            get { return ((ClosableHeader)Header).txtHeader.Text; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    ((ClosableHeader)Header).txtHeader.Text = value;
                }
            }
        }

        public ClosableTabItem()
        {
            Header = new ClosableHeader();
            ((ClosableHeader)Header).txtHeader.Text = TextNewTab;
            ((ClosableHeader)Header).btnClose.Click += btnClose_Click;

            this.MaxWidth = 150;
            this.AllowDrop = true;

            this.MouseDown += ClosableTabItem_MouseDown;
            this.Drop += ClosableTabItem_Drop;

            Content = new StackPanel();
            ((Panel)Content).Children.Add(new CheckBox() { Content = new TextBlock() { Text = "Test CheckBox" } });
            ((Panel)Content).Children.Add(new Button() { Content = new TextBlock() { Text = "Test Button" } });
            ((Panel)Content).Children.Add(new TextBlock() { Text = "Test TextBlock" });

        }

        void ClosableTabItem_Drop(object sender, DragEventArgs e)
        {
            var targetItem = sender as ClosableTabItem;
            var parent = targetItem.Parent as ExtendedTabControl;
            int newIndex = parent.Items.IndexOf(targetItem);
            //source item can't be _addsTab.
            var sourceItem = e.Data.GetData(typeof(ClosableTabItem)) as ClosableTabItem;

            //_addsTab must be last in Items
            if (targetItem == parent.AddsTab)
                newIndex--;

            parent.Items.Remove(sourceItem);
            parent.Items.Insert(newIndex, sourceItem);
            parent.SelectedItem = sourceItem;

            parent.OnDragDropNow = false;
        }

        void ClosableTabItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ClosableTabItem;
            var parent = item.Parent as ExtendedTabControl;

            if (item == parent.AddsTab)
                return;

            parent.OnDragDropNow = true;

            DragDrop.DoDragDrop(sender as ClosableTabItem, sender, DragDropEffects.Move);
        }


        void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CloseTabFrom(this.Parent, this);
        }

        public static void AddNewClosableTabItemTo(object tabControl)
        {
            if (tabControl == null) throw new ArgumentNullException();
            if (tabControl is ExtendedTabControl)
            {
                var tab = new ClosableTabItem();
                (tabControl as ExtendedTabControl).Items.Insert((tabControl as ExtendedTabControl).Items.Count - 1, tab);
                (tabControl as ExtendedTabControl).SelectedItem = tab;
            }
        }

        public static void CloseTabFrom(object parent, ClosableTabItem tab)
        {
            if (parent is ExtendedTabControl)
            {
                var etc = parent as ExtendedTabControl;
                etc.SelectedIndex = etc.Items.IndexOf(tab) - 1;

                (tab.Header as ClosableHeader).txtHeader = null;
                (tab.Header as ClosableHeader).btnClose = null;
                tab.Header = null;

                RecursiveClearPanelsChildren(tab.Content as Panel);

                etc.Items.Remove(tab);
            }
        }

        private static void RecursiveClearPanelsChildren(Panel p)
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
        }

        public static ClosableTabItem transformToClosableTabItem(object value)
        {
            if (value == null) throw new ArgumentNullException();
            if (value is ClosableTabItem)
            {
                return value as ClosableTabItem;
            }
            else
            {
                return new ClosableTabItem() { Content = value, HeaderText = TextNewTab };
            }
        }

        public static IList<ClosableTabItem> transformToClosableTabItem(IList<object> values)
        {
            if (values == null) throw new ArgumentNullException();
            var newItems = new List<ClosableTabItem>();
            for (int i = 0; i < values.Count; i++)
            {
                newItems.Add(transformToClosableTabItem(values[i]));
            }

            return newItems;
        }
    }
}
