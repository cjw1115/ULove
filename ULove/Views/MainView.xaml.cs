using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class MainView : Page
    {
        private static int count = 0;
        public MainView()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;
            //this.Loaded += MainView_Loaded;   //测试使用，动态设置服务主机地址
            
        }

        private async void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            if (count == 0)
            {
                dlg.PrimaryButtonText = "设置";
                dlg.PrimaryButtonClick += (o, arg) => { App.rootUri = tb.Text; };
                await dlg.ShowAsync();
            }
            count++;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            
            
            GetNavigateToCommand(this)?.Execute(e.Parameter);

        }
       
        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }
        public static readonly DependencyProperty NavigateToCommandProperty = DependencyProperty.Register("NavigateToCommand", typeof(ICommand), typeof(MainView), new PropertyMetadata(null));
        public static ICommand GetNavigateToCommand(DependencyObject o)
        {
            return (ICommand)o.GetValue(NavigateToCommandProperty);
        }
        public static void SetNavigateToCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(NavigateToCommandProperty, value);
        }

       
    }

    public class ListViewEx : DependencyObject
    {
        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register("ItemClickCommand", typeof(ICommand), typeof(ListViewEx), new PropertyMetadata(null, OnPropertyChanged));
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listView = (ListView)d;
            listView.ItemClick += ListView_ItemClick;
        }

        private static void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var listView = (ListView)sender;
            var selected = listView.Resources["selected"];
            var unselected = listView.Resources["unselected"];
            ICommand clickCommand = (ICommand)listView.GetValue(ItemClickCommandProperty);

            int index = -1;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                ListViewItem item = listView.Items[i] as ListViewItem;
                if(item.Content == e.ClickedItem)
                {
                    index = i;
                    item.Style = (Style)selected;
                    
                }
                else
                {
                    item.Style = (Style)unselected;
                }
                

            } 
            clickCommand?.Execute(index.ToString());
            
        }
        public static ICommand GetItemClickCommand(DependencyObject o)
        {
            return (ICommand)o.GetValue(ItemClickCommandProperty);
        }
        public static void  SetItemClickCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(ItemClickCommandProperty, value);
        }
      
        
    }

}
