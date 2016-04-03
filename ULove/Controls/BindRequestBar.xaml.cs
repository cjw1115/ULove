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
    public sealed partial class BindRequestBar : UserControl
    {
        public BindRequestBar()
        {
            this.InitializeComponent();
            this.mainGrid.DataContext = this;
            this.Visibility = Visibility.Collapsed;
        }
        public static readonly DependencyProperty BindRequestIDProperty = DependencyProperty.Register("BindRequestID", typeof(string), typeof(BindRequestBar), new PropertyMetadata(null, (o, e) => 
        {
            var bar = o as BindRequestBar;
            if (string.IsNullOrEmpty(bar.BindRequestID))
            {
                bar.Visibility = Visibility.Collapsed;
            }
            else
            {
                bar.Visibility = Visibility.Visible;
            }
        }));

        public string BindRequestID
        {
            get { return (string)GetValue(BindRequestIDProperty); }
            set { SetValue(BindRequestIDProperty, value); }
        }

        public static readonly DependencyProperty AcceptCommandProperty = DependencyProperty.Register("AcceptCommand", typeof(ICommand), typeof(BindRequestBar), new PropertyMetadata(null, (o, e) =>
        {
            var bar = o as BindRequestBar;
            bar.btnAccept.Command = bar.AcceptCommand;
        }));
        public ICommand AcceptCommand
        {
            get { return (ICommand)GetValue(AcceptCommandProperty); }
            set { SetValue(AcceptCommandProperty, value); }
        }

        public static readonly DependencyProperty RefuseCommandProperty = DependencyProperty.Register("RefuseCommand", typeof(ICommand), typeof(BindRequestBar), new PropertyMetadata(null, (o, e) =>
        {
            var bar = o as BindRequestBar;
            bar.btnRefuse.Command = bar.RefuseCommand;
        }));
        public ICommand RefuseCommand
        {
            get { return (ICommand)GetValue(RefuseCommandProperty); }
            set { SetValue(RefuseCommandProperty, value); }
        }
    }
}
