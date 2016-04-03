using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ULove.Models;
using ULove.Services;
using ULove.Views;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using ULove.Manager;

namespace ULove.ViewModels
{
    public class OnlineImageVM:ViewModelBase
    {
        private OnlineImages _onlineImages;
        public OnlineImages OnlineImages
        {
            get { return _onlineImages; }
            set
            {
                Set(ref _onlineImages, value);
            }
        }
        public  OnlineImageManager onlineImageManager { get; set; }
        

        private Visibility _progressRingVisibility;
        public Visibility ProgressRingVisibility
        {
            get
            {
                return _progressRingVisibility;
            }

            set
            {
                Set(ref _progressRingVisibility, value);
            }
        }

        public ICommand OpenCommand { get; set; }
        public void Open(object param)
        {
            if (param == null)
                return;
            //ImageEntity param = SelectedItem as ImageEntity;
            ULoveImageEntity entity = param as ULoveImageEntity;
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.ULoveImageShow.ULoveImage = entity;
            locator.Navigation.NavigateTo(typeof(ULoveImageShowView).Name);
        }

        public ICommand NavigateToCommand { get; set; }
        private async  void NavigateTo()
        {
            if(this.OnlineImages==null|| OnlineImages.Count == 0)
            {
                LoadImages();
            }
            else
            {
                await OnlineImages.LoadMoreItemsAsync(1);
              
            }
        }
        public OnlineImageVM()
        {
            onlineImageManager = new OnlineImageManager();
            //LoadImages();
            OpenCommand = new RelayCommand<object>(Open);
            NavigateToCommand = new RelayCommand(NavigateTo);

        }
        public async void LoadImages()
        {
            if (OnlineImages == null)
            {
                OnlineImages = new OnlineImages();
                OnlineImages.LoadMoreBegain += (o, e) => { ProgressRingVisibility = Visibility.Visible; };
                OnlineImages.LoadMoreEnd += (o, e) => { ProgressRingVisibility = Visibility.Collapsed; };
            }
            
        }
        
    }
}
