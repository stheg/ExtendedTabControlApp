using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExtendedTabControlApp
{
    public class ExtendedTabControl : TabControl
    {
        private readonly static TabItemFactory _factory = new ClosableTabItemFactory();
        private readonly static TabItemCleaner _cleaner = new ClosableTabItemCleaner();

        private readonly ClosableTabItem _addsTab = (_factory.CreateTabItem() as ClosableTabItem).TransformToAddsTab();
        /// <summary>
        /// Special TabItem that represent adding of other TabItems
        /// </summary>
        public ClosableTabItem AddsTab { get { return _addsTab; } }

        private List<Key> _pressedKeys = new List<Key>();

        private bool _isDragDropNow = false;
        public bool IsDragDropNow { get { return _isDragDropNow; } set { _isDragDropNow = value; } }

        /// <summary>
        /// TabControl with auto-resize of TabItems and special AddsTab
        /// </summary>
        public ExtendedTabControl()
        {
            this.Template = FindResource("extendedTabControlTemplate") as ControlTemplate;
            this.Loaded += ExtendedTabControl_Loaded;
            this.KeyDown += ExtendedTabControl_KeyDown;
            this.SelectionChanged += ExtendedTabControl_SelectionChanged;
        }

        void ExtendedTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] == _addsTab && !_isDragDropNow)
            {
                AddTabItem();
                (this.Items[this.Items.Count - 2] as ClosableTabItem).Focus();
            }
        }

        void ExtendedTabControl_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                if (e.Key == Key.W)
                {
                    CloseTabItem(this.SelectedItem as ClosableTabItem);
                }
                if (e.Key == Key.T)
                {
                    AddTabItem();
                }
            }
        }

        /// <summary>
        /// Close TabItem and remove it from a memory
        /// </summary>
        /// <param name="tab"></param>
        public void CloseTabItem(TabItem tab)
        {
            this.SelectedIndex = this.Items.IndexOf(tab) - 1;

            _cleaner.ClearTabItem(tab);
            
            this.Items.Remove(tab);
        }

        /// <summary>
        /// Adds TabItem on position before the AddsTab
        /// </summary>
        public void AddTabItem()
        {
            this.Items.Insert(this.Items.Count - 1, _factory.CreateTabItem(true));
        }

        void ExtendedTabControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.AddChild(_factory.CreateTabItem(true));
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
