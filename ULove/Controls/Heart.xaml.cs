using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ULove.Controls
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Heart : Page
    {
        public Heart()
        {
            this.InitializeComponent();
            this.grid.DataContext = this;
            this.Visibility = Visibility.Collapsed;
            this.BindStoryBoard.Completed += BindStoryBoard_Completed;
        }

        private void BindStoryBoard_Completed(object sender, object e)
        {
            this.Show = false;
        }

        public static readonly DependencyProperty ShowProperty = DependencyProperty.Register("Show", typeof(bool), typeof(Heart), new PropertyMetadata(false, (o, e) =>
              {
                  var heart = o as Heart;
                  if (heart.Show)
                  {
                      heart.Visibility = Visibility.Visible;
                      heart.BindStoryBoard.Begin();
                  }
                  else
                  {
                      heart.Visibility = Visibility.Collapsed;
                  }
              }));
        public bool Show
        {
            get { return (bool)GetValue(ShowProperty); }
            set { SetValue(ShowProperty, value); }
        }
    }
}
