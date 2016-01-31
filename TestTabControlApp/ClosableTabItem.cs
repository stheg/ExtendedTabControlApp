﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TestTabControlApp
{
    public class ClosableTabItem : TabItem
    {
        private const String AddTagText = "+";
        private const String NewTagText = "Новая вкладка";

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
                    Header = new TextBlock() { Text = AddTagText };
                }
            }
        }

        public ClosableTabItem()
        {
            Header = new ClosableHeader();
            ((ClosableHeader)Header).txtHeader.Text = NewTagText;
            ((ClosableHeader)Header).btnClose.Click += btnClose_Click;

            //test example
            Content = new StackPanel();
            ((Panel)Content).Children.Add(new CheckBox() { Content = new TextBlock() { Text = "Test CheckBox" } });
            ((Panel)Content).Children.Add(new Button() { Content = new TextBlock() { Text = "Test Button" } });
            ((Panel)Content).Children.Add(new TextBlock() { Text = "Test TextBlock" });
        }

        void btnClose_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var parent = (TabControl)this.Parent;
            ((Panel)Content).Children.Clear();
            parent.SelectedIndex = parent.Items.IndexOf(this) - 1;
            parent.Items.Remove(this);
            resize(parent);
        }

        protected override void OnSelected(RoutedEventArgs e)
        {
            if (_isAddTab)
            {
                var parent = (TabControl)this.Parent;
                base.OnUnselected(e);
                parent.Items.Insert(parent.Items.Count - 1, new ClosableTabItem());
                parent.SelectedIndex = parent.Items.Count - 2;
                resize(parent);
            }
            else
            {
                base.OnSelected(e);
            }
        }

        private void resize(TabControl parent)
        {
            return;
            var items = parent.Items;
            double itemsWidth = 0.0d;
            foreach (Control item in items)
            {
                itemsWidth += item.ActualWidth;
            }
            if (itemsWidth > parent.ActualWidth)
            {
                for (int i = 0; i < items.Count-1; i++)
                {
                    ((Control)items[i]).Width = parent.ActualWidth / items.Count;
                }
            }
        }
    }
}
