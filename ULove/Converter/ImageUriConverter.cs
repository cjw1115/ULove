using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace ULove.Converter
{
    public class ImageUriConverter : IValueConverter
    {
        public  object  Convert(object value, Type targetType, object parameter, string language)
        {
            string uri_s = value as string;
            if (string.IsNullOrEmpty(uri_s))
            {
                
                BitmapImage bitmap = new BitmapImage(new Uri("ms-appx:///images//u.jpg"));

                return bitmap;
            }
            else
            {
                var uri= new Uri(uri_s); 
                BitmapImage bitmap = new BitmapImage(uri);
                
                return bitmap;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        
    }
}
