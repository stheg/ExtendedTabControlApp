using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            this.AllowDrop = true;

            //test example
            Content = new StackPanel();
            ((Panel)Content).Children.Add(new CheckBox() { Content = new TextBlock() { Text = "Test CheckBox" } });
            ((Panel)Content).Children.Add(new Button() { Content = new TextBlock() { Text = "Test Button" } });
            ((Panel)Content).Children.Add(new TextBlock() { Text = "Test TextBlock" });
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
                var tc = (TabControl)parent;
                tc.SelectedIndex = tc.Items.IndexOf(tab) - 1;
                ((Panel)tab.Content).Children.Clear();
                tc.Items.Remove(tab);
            }
        }

        public static ClosableTabItem transformToClosableTabItem(object value)
        {
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
            var newItems = new List<ClosableTabItem>();
            for (int i = 0; i < values.Count; i++)
            {
                newItems.Add(transformToClosableTabItem(values[i]));
            }

            return newItems;
        }
    }
}
