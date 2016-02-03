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

namespace TestTabControlApp
{
    public class ClosableTabItem : TabItem
    {
        private const String AddTagText = "+";
        private const String NewTabText = "Новая вкладка";

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

        private bool _isAddTab = false;
        public bool IsAddTab 
        {
            get { return _isAddTab; }
            set
            {
                _isAddTab = value;
                if (_isAddTab)
                {
                    Header = new ClosableHeader() { Content = new TextBlock() { Text = "+" } };
                    MaxWidth = 25;
                    Width = 25;
                }
            }
        }

        public ClosableTabItem()
        {
            Header = new ClosableHeader();
            ((ClosableHeader)Header).txtHeader.Text = NewTabText;
            ((ClosableHeader)Header).btnClose.Click += btnClose_Click;

            MaxWidth = 150;

            this.MouseDown += ClosableTabItem_MouseDown;
            this.AllowDrop = true;
            this.Drop += ClosableTabItem_Drop;

            //test example
            Content = new StackPanel();
            ((Panel)Content).Children.Add(new CheckBox() { Content = new TextBlock() { Text = "Test CheckBox" } });
            ((Panel)Content).Children.Add(new Button() { Content = new TextBlock() { Text = "Test Button" } });
            ((Panel)Content).Children.Add(new TextBlock() { Text = "Test TextBlock" });
            ((Panel)Content).Children.Add(new Image() { Source = new BitmapImage(new Uri("Resources/IMG_7508.jpg", UriKind.Relative)) { CreateOptions = BitmapCreateOptions.IgnoreImageCache } });
        }

        void ClosableTabItem_Drop(object sender, DragEventArgs e)
        {
            var targetItem = sender as ClosableTabItem;
            var parent = targetItem.Parent as ExtendedTabControl;
            int newIndex = parent.Items.IndexOf(targetItem);
            var sourceItem = e.Data.GetData(typeof(ClosableTabItem)) as ClosableTabItem;

            if (sourceItem.IsAddTab)
                return;
            //addsTab must be last in Items
            if (targetItem.IsAddTab)
                newIndex--;

            parent.Items.Remove(sourceItem);
            parent.Items.Insert(newIndex, sourceItem);
            parent.SelectedIndex = -1;
            parent.SelectedItem = sourceItem;
        }

        void ClosableTabItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if ((sender as ClosableTabItem).IsAddTab)
                return;
            DragDrop.DoDragDrop(sender as ClosableTabItem, sender, DragDropEffects.Move);
        }


        void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CloseTabFrom(this.Parent, this);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (_isAddTab)
            {
                OnUnselected(e);
                AddNewClosableTabItemTo(this.Parent);
            }
            else
            {
                base.OnSelected(e);
            }
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
                var tc = parent as ExtendedTabControl;
                tc.SelectedIndex = tc.Items.IndexOf(tab) - 1;
                
                (tab.Header as ClosableHeader).txtHeader = null;
                (tab.Header as ClosableHeader).btnClose = null;
                tab.Header = null;

                RecursiveClearPanelsChildren(tab.Content as Panel);
                
                tc.Items.Remove(tab);
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
                return new ClosableTabItem() { Content = value, HeaderText = NewTabText };
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
