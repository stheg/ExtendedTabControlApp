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
        private const String TAG_ADD_TAB_TEXT = "+";
        private const String TAG_NEW_TAB_TEXT = "Новая вкладка";
        private const double DEFAULT_MAX_WIDTH = 150.0d;
        private const double DEFAULT_MAX_WIDTH_ADDS_TAB = 25.0d;

        /// <summary>
        /// Text that appears in Header of TabItem
        /// </summary>
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

        /// <summary>
        /// Extended version of TabItem which has special Header (Drag'n'Drop operations enabled)
        /// </summary>
        public ClosableTabItem()
        {
            Header = new ClosableHeader();
            ((ClosableHeader)Header).txtHeader.Text = TAG_NEW_TAB_TEXT;
            ((ClosableHeader)Header).btnClose.Click += btnClose_Click;

            this.MaxWidth = DEFAULT_MAX_WIDTH;
            this.AllowDrop = true;

            this.MouseDown += ClosableTabItem_MouseDown;
            this.Drop += ClosableTabItem_Drop;

            //example of content
            Content = new StackPanel();
            ((Panel)Content).Children.Add(new CheckBox() { Content = new TextBlock() { Text = "Test CheckBox" } });
            ((Panel)Content).Children.Add(new Button() { Content = new TextBlock() { Text = "Test Button" } });
            ((Panel)Content).Children.Add(new TextBlock() { Text = "Test TextBlock" });

        }

        /// <summary>
        /// Transform this instance to special ClosableTabItem which have default Header and Width (Drag operation disabled).
        /// </summary>
        /// <returns></returns>
        public ClosableTabItem TransformToAddsTab()
        {
            this.Header = new TextBlock() { Text = TAG_ADD_TAB_TEXT };
            this.Width = DEFAULT_MAX_WIDTH_ADDS_TAB;
            this.MaxWidth = DEFAULT_MAX_WIDTH_ADDS_TAB;
            this.Content = null;

            return this;
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

            parent.IsDragDropNow = false;
        }

        void ClosableTabItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var item = sender as ClosableTabItem;
            var parent = item.Parent as ExtendedTabControl;

            if (item == parent.AddsTab)
                return;

            parent.IsDragDropNow = true;

            DragDrop.DoDragDrop(sender as ClosableTabItem, sender, DragDropEffects.Move);
        }


        void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            (this.Parent as ExtendedTabControl).CloseTabItem(this);
        }

        /// <summary>
        /// Transorm any object to ClosableTabItem with default HeaderText where Content equals to the passed object 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ClosableTabItem transformToClosableTabItem(object value)
        {
            if (value == null) throw new ArgumentNullException();
            if (value is ClosableTabItem)
            {
                return value as ClosableTabItem;
            }
            else
            {
                return new ClosableTabItem() { Content = value, HeaderText = TAG_NEW_TAB_TEXT };
            }
        }

        /// <summary>
        /// Transform list of any objects to IList with ClosableTabItems
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
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
