﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class ULoveImageShowView : Page
    {
        public ULoveImageShowView()
        {
            this.InitializeComponent();
            var frame=Window.Current.Content as Frame;
            GalaSoft.MvvmLight.Messaging.Messenger.Default.Register<string>(this,
                 (s) =>
                {
                    notificationBar.ShowMessage(s);
                   
                });
            
        }
    }
}
