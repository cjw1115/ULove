using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ULove.Converter
{
    class GenderConverter : IValueConverter
    {
        private readonly string male = "ms-appx:///images/male.png";
        private readonly string female = "ms-appx:///images/female.png";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                
                string gender = value as string;
                gender = gender.ToLower();
                if (gender == "m")
                {
                    return male;
                }
                else if(gender=="f")
                {
                    return female;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null ;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
