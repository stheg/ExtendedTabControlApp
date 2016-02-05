using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestTabControlApp
{
    public class ExtendedTabControl : TabControl
    {
        private readonly ClosableTabItem _addsTab = new ClosableTabItem() { IsAddTab = true };

        private List<Key> _pressedKeys = new List<Key>();

        public ExtendedTabControl()
            : base()
        {
            this.Template = FindResource("extendedTabControlTemplate") as ControlTemplate;
            base.Loaded += ExtendedTabControl_Loaded;
            this.KeyDown += ExtendedTabControl_KeyDown;
        }

        void ExtendedTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.W)
                {
                    ClosableTabItem.CloseTabFrom(this, this.SelectedItem as ClosableTabItem);
                }
                if (e.Key == Key.T)
                {
                    ClosableTabItem.AddNewClosableTabItemTo(this);
                }
            }
        }

        void ExtendedTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddChild(new ClosableTabItem() { IsSelected = true });
            this.AddChild(_addsTab);
        }

        protected override void OnItemsChanged(System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            resizeItems();
        }

        protected override void OnRenderSizeChanged(System.Windows.SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            resizeItems();
        }

        private void resizeItems()
        {
            foreach (FrameworkElement item in this.Items)
            {
                item.Width = this.ActualWidth / this.Items.Count;
            }
        }
    }
}
