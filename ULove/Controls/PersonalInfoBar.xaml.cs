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
using ULove.Models;
using System.ComponentModel;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ULove.Controls
{
    public sealed partial class PersonalInfoBar : UserControl
    {
        public PersonalInfoBar()
        {
            this.InitializeComponent();
            this.gridMain.DataContext = this;
            if (this.User == null)
            {
                this.Visibility = Visibility.Collapsed;
            }
        }
        public UserEntity User
        {
            get { return (UserEntity)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register("User", typeof(UserEntity), typeof(PersonalInfoBar), new PropertyMetadata(null, (o, e) =>
        {
            var infobar = o as PersonalInfoBar;
            
            if (infobar.User==null)
            {
                infobar.Visibility = Visibility.Collapsed;
            }
            else
            {
                //infobar.DataContext = infobar.User;
                infobar.Visibility = Visibility.Visible;
            }
        }));
    }
}
