using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ULove.Views;
using Windows.UI.Xaml;

namespace ULove.ViewModels
{
    public class SinaWeiboLoginVM:ViewModelBase
    {
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                Set(ref _code, value);
                if (value != null)
                    Navigate();
            }
        }

        private void Navigate()
        {
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.Navigation.NavigateTo(typeof(MainView).Name);
        }
    }
}
