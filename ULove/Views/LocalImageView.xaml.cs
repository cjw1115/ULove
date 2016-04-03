using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace ULove.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LocalImageView : Page
    {
        public LocalImageView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required; 
        }

        //private void gridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (e.AddedItems.Count > 0)
        //    {
        //        for (int i = 0; i < e.AddedItems.Count; i++)
        //        {
        //            var item = e.AddedItems[i];
        //            GridViewItem gridViewItem = gridView.ContainerFromItem(item) as GridViewItem;
        //            if(gridViewItem!=null)
        //                gridViewItem.ContentTemplate = Selected;
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < e.RemovedItems.Count; i++)
        //        {
        //            var item = e.RemovedItems[i];
                    
        //            GridViewItem gridViewItem = gridView.ContainerFromItem(item) as GridViewItem;
        //            if (gridViewItem != null)
        //                gridViewItem.ContentTemplate = Normal;
                    
        //        }

        //    }
        //}

        private childItem FindVisualChild<childItem>(DependencyObject obj) where childItem : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is childItem)
                    return (childItem)child;
                else
                {
                    childItem childOfChild = FindVisualChild<childItem>(child);
                    if (childOfChild != null)
                        return childOfChild;
                }
            }
            return null;
        }

        private void btnSelectAlls_Click(object sender, RoutedEventArgs e)
        {
            if (gridView.Items.Count == 0)
                return;
            if(gridView.SelectedItems.Count!=gridView.Items.Count)
            {
                gridView.SelectAll();
            }
            else
            {
                gridView.DeselectRange(new ItemIndexRange(0, (uint)(gridView.Items.Count - 1)));
            }
        }

        public static readonly DependencyProperty OnloadCommandProperty = DependencyProperty.Register("OnloadCommand", typeof(ICommand), typeof(LocalImageView), new PropertyMetadata(null,
            (o, e) =>
            {
                LocalImageView page = o as LocalImageView;
                if(page!=null)
                {
                    page.Loaded += Page_Loaded;
                }
            }));

        private static void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var command = GetOnloadCommand(sender as DependencyObject); command?.Execute(e);
        }

        public  static ICommand GetOnloadCommand(DependencyObject o)
        {
            LocalImageView page = o as LocalImageView;
            page.Loaded -= Page_Loaded;
            return (ICommand)o.GetValue(OnloadCommandProperty);
        }
        public static void SetOnloadCommand(DependencyObject o, ICommand value)
        {
            
        }
    }
}
