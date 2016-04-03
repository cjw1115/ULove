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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace ULove.Controls
{
    public sealed partial class PushLoverDialog : UserControl
    {
        private Frame _frame;
        public PushLoverDialog()
        {
            InitializeComponent();
            this.Visibility = Visibility.Collapsed;
            btnOk.Click += (sender, arg) => { OkCommand?.Execute(this.Message.Text); CloseDlgStoryboard.Begin(); };
            btnClose.Click += (sender, arg) => { CancelCommand?.Execute(null); CloseDlgStoryboard.Begin(); };
            
        }
        public static readonly DependencyProperty OkCommandProperty = DependencyProperty.Register("OkCommand", typeof(ICommand), typeof(PushLoverDialog), new PropertyMetadata(null));
        public ICommand OkCommand
        {
            get { return (ICommand)GetValue(OkCommandProperty); }
            set { SetValue(OkCommandProperty, value); }
        }
        private static T FindElement<T>(DependencyObject o) where T : class
        {
            Queue<DependencyObject> queue = new Queue<DependencyObject>();
            queue.Enqueue(o);
            while (queue.Count > 0)
            {
                var root = queue.Dequeue();
                if (root.GetType() == typeof(Popup))
                    root = ((Popup)root).Child;
                var count = VisualTreeHelper.GetChildrenCount(root);

                for (int i = 0; i < count; i++)
                {
                    var item = VisualTreeHelper.GetChild(root, i);
                    var goal = item as T;
                    if (goal != null)
                    {
                        return goal;
                    }
                    else
                    {
                        queue.Enqueue(item);
                    }
                }
            }
            return null;
        }

        public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register("CancelCommand", typeof(ICommand), typeof(PushLoverDialog), new PropertyMetadata(null));
        public ICommand CancelCommand
        {
            get { return (ICommand)GetValue(CancelCommandProperty); }
            set { SetValue(CancelCommandProperty, value); }
        }

      
        public static readonly DependencyProperty OpenProperty = DependencyProperty.Register("Open", typeof(bool), typeof(PushLoverDialog), new PropertyMetadata(false, (o, e) =>
        {
            var dlg = o as PushLoverDialog;

            var isopen = dlg.Open;
            if (isopen == true)
            {
                dlg.OpenDlgStoryboard.Begin();
                
            }
        }));
        public bool Open
        {
            get { return (bool)GetValue(OpenProperty); }
            set { SetValue(OpenProperty, value); }
        }
    }
}
