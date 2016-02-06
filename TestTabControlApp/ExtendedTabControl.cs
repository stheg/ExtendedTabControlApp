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
        private readonly ClosableTabItem _addsTab = new ClosableTabItem() 
        { 
            Header = new TextBlock() { Text = "+" },
            Content = null,
            Width = 25.0d, MaxWidth = 25.0d 
        };

        public ClosableTabItem AddsTab { get { return _addsTab; } }

        private List<Key> _pressedKeys = new List<Key>();

        private bool _onDragDrop = false;
        public bool OnDragDropNow { get { return _onDragDrop; } set { _onDragDrop = value; } }

        public ExtendedTabControl()
            : base()
        {
            this.Template = FindResource("extendedTabControlTemplate") as ControlTemplate;
            this.Loaded += ExtendedTabControl_Loaded;
            this.KeyDown += ExtendedTabControl_KeyDown;
            this.SelectionChanged += ExtendedTabControl_SelectionChanged;
        }

        void ExtendedTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] == _addsTab && !_onDragDrop)
            {
                ClosableTabItem.AddNewClosableTabItemTo(this);
                (this.Items[this.Items.Count - 2] as ClosableTabItem).Focus(); 
            }
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
