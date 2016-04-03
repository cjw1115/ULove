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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ULove.Controls
{
    public sealed partial class MyTextBox : UserControl
    {
        
        public MyTextBox()
        {
            this.InitializeComponent();
            this.mainGrid.DataContext = this;
            //this.DataContext = this;
            this.tbfore.TextChanged += Tbfore_TextChanged;
            
        }
        
        private void Tbfore_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.tbfore.Text))
            {
                this.tbback.Visibility = Visibility.Visible;
            }
            else
            {
                this.tbback.Visibility = Visibility.Collapsed;
            }
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(MyTextBox), null);
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty,value); }
        }

        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register("WaterMark", typeof(string), typeof(MyTextBox), null);
        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        private void tbback_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = false;
        }
    }
}
