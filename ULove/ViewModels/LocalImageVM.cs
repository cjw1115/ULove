using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ULove.Models;
using ULove.Services;
using ULove.Views;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ULove.Manager;

namespace ULove.ViewModels
{
    public class LocalImageVM:ViewModelBase
    {
        private ObservableCollection<ULoveImageEntity> _localImages;
        public ObservableCollection<ULoveImageEntity> LocalImages
        {
            get { return _localImages; }
            set
            {
                Set(ref _localImages, value);
            }
        }

        private object _selectedItem;
        public object SelectedItem
        {
            get { return _selectedItem; }
            set { Set(ref _selectedItem, value); }
        }
        public LocalImagesManager localImagesManager { get; set; }

        private StorageFolder _localImageFolder;
        public StorageFolder LocalImageFolder
        {
            get { return _localImageFolder; }
            set { _localImageFolder = value; /*LoadImages();*/ }
        }
        
        /// <summary>
        /// 向本地数据库添加图片
        /// </summary>
        //public async void LoadImages()
        //{
        //    await localImagesManager.LoadImages(LocalImageFolder);
            
        //}
        /// <summary>
        /// 从本地数据库获取图片
        /// </summary>
        public async void GetImages()
        {
            var images = await localImagesManager.GetImages();
            LocalImages = images;

        }

        private Visibility _btnSelectVisibility;
        public Visibility BtnSelectVisibility
        {
            get
            {
                return _btnSelectVisibility;
            }

            set
            {
                Set(ref _btnSelectVisibility, value);
            }
        }
        public ICommand SelectCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand ShareCommand { get; set; }
        public ICommand OpenCommand { get; set; }
        public ICommand OnLoadCommand { get; set; }

        public void Select()
        {
            BtnSelectVisibility = Visibility.Collapsed;

            //不可点击，可多选
            IsItemClickEnabled = false;
            SelectionMode = ListViewSelectionMode.Multiple;
        }
        public void Delelte(IList<object> items)
        {
            ObservableCollection<ULoveImageEntity> selectedImgs = new ObservableCollection<ULoveImageEntity>();
            foreach (var item in items)
            {
                var img=item as ULoveImageEntity;
                selectedImgs.Add(img);
            }
            foreach (var item in selectedImgs)
            {
                LocalImages.Remove(item);
                localImagesManager.DeleteImage(item);
            }

            BtnSelectVisibility = Visibility.Visible;
            IsItemClickEnabled = true;
            SelectionMode = ListViewSelectionMode.None;
        }
        public void Refresh()
        {
            GetImages();
        }
        public void Share()
        {
            BtnSelectVisibility = Visibility.Visible;
            IsItemClickEnabled = true;
            SelectionMode = ListViewSelectionMode.None; ;
        }
        public void Open(object param)
        {
            if (param == null)
                return;
            //ImageEntity param = SelectedItem as ImageEntity;
            ULoveImageEntity entity = param as ULoveImageEntity;
            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            locator.EditImage.ImageEntity = entity;
            locator.Navigation.NavigateTo(typeof(EditImage).Name);
        }
        /// <summary>
        /// 页面加载
        /// </summary>
        public void OnLoad()
        {
            GetImages();
        }
        public LocalImageVM()
        {
            localImagesManager = new LocalImagesManager();

            OnLoadCommand = new RelayCommand(OnLoad);

            SelectCommand = new RelayCommand(Select);
            DeleteCommand = new RelayCommand<IList<object>>(Delelte);
            ShareCommand = new RelayCommand(Share);
            RefreshCommand = new RelayCommand(Refresh);
            OpenCommand = new RelayCommand<object>(Open);
            //默认可点击
            IsItemClickEnabled = true;
            SelectionMode = ListViewSelectionMode.None;
        }

        private bool _isItemClickEnabled;
        private ListViewSelectionMode _selectionMode;
        public bool IsItemClickEnabled
        {
            get
            {
                return _isItemClickEnabled;
            }

            set
            {
                Set(ref _isItemClickEnabled, value);
            }
        }
        public ListViewSelectionMode SelectionMode
        {
            get
            {
                return _selectionMode;
            }

            set
            {
                Set(ref _selectionMode, value);
            }
        }
        
    }
}
