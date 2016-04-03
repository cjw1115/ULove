using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ULoveImageControl : UserControl
    {
        
        public ULoveImageControl()
        {
            this.InitializeComponent();
            this.flipView.SelectionChanged += FlipView_SelectionChanged;
        }

        private void FlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.SetValue(CurrentImageProperty, this.flipView.SelectedItem);
        }

        public IList<ImageSource> UloveImages
        {
            get { return (IList<ImageSource>)GetValue(UloveImagesProperty); }
            set { SetValue(UloveImagesProperty, value); }
            
        }
        public static readonly DependencyProperty UloveImagesProperty = DependencyProperty.RegisterAttached("UloveImages", typeof(IList<ImageSource>), typeof(ULoveImageControl), new PropertyMetadata(null, (o, e) =>
         {
             SetUloveImages(o, e.NewValue);
         }));

        //public static IList<ImageSource> GetUloveImages(DependencyObject sender)
        //{
        //    FlipView flipView = FindElement<FlipView>(sender);
        //    return flipView.ItemsSource as IList<ImageSource>;
        //}
        public static void SetUloveImages(DependencyObject sender, object value)
        {
            IList<ImageSource> list = value as IList<ImageSource>;
            if (list == null)
                return;
            FlipView flipView = FindElement<FlipView>(sender);
            flipView.ItemsSource = value;
        }
        public static T FindElement<T>(DependencyObject o) where T : class
        {
            Queue<DependencyObject> queue = new Queue<DependencyObject>();
            queue.Enqueue(o);
            var count = VisualTreeHelper.GetChildrenCount(o);

            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                if (item is T)
                    return item as T;
                else
                {
                    for (int i = 0; i < VisualTreeHelper.GetChildrenCount(item); i++)
                    {
                        queue.Enqueue(VisualTreeHelper.GetChild(item, i));
                    }
                }
            }
            return null;
        }

        

        public ImageSource CurrentImage
        {
            get{ return (ImageSource)GetValue(CurrentImageProperty); }
            set { SetValue(CurrentImageProperty, value); }
        }
        public static readonly DependencyProperty CurrentImageProperty = DependencyProperty.Register("CurrentImage", typeof(ImageSource), typeof(ULoveImageControl), null);
    }
}
