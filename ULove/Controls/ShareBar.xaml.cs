using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ULove.Controls
{

    public sealed partial class ShareBar : UserControl
    {

        public ShareBar()
        {
            
            this.InitializeComponent();
            this.gridMain.DataContext = this;
        }
        public static readonly DependencyProperty ULoveImageProperty = DependencyProperty.Register("ULoveImage", typeof(ULove.Models.ULoveImage), typeof(ShareBar), new PropertyMetadata(null,(o,e)=> {

        }));
        public ULove.Models.ULoveImage ULoveImage
        {
            get { return (ULove.Models.ULoveImage)GetValue(ULoveImageProperty); }
            set { SetValue(ULoveImageProperty, value); }
        }

        public static readonly DependencyProperty ULoveSharedInfoProperty = DependencyProperty.Register("ULoveSharedInfo", typeof(Models.ULoveShareEntity), typeof(ShareBar), new PropertyMetadata(null));
        public Models.ULoveShareEntity ULoveSharedInfo
        {
            get { return (Models.ULoveShareEntity)GetValue(ULoveSharedInfoProperty); }
            set { SetValue(ULoveSharedInfoProperty, value); }
        }

        public static readonly DependencyProperty ClickProperty = DependencyProperty.Register("Click", typeof(ICommand), typeof(ShareBar), new PropertyMetadata(null, (o, e) =>
        {
            var shareBar = o as ShareBar;
            shareBar.Tapped += ShareBar_Tapped;
        }));

        private static void ShareBar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var shareBar = sender as ShareBar;
            shareBar.Click?.Execute(null);
        }

        public ICommand Click
        {
            get { return (ICommand)GetValue(ClickProperty); }
            set { SetValue(ClickProperty, value); }
        }
    }
}
