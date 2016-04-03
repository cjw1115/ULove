using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ULove.Services;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using ULove.Manager;

namespace ULove.Models
{
    public class OnlineImages : ObservableCollection<ULoveImageEntity>, ISupportIncrementalLoading
    {

        public bool HasMoreItems
        {
            get
            {
                return HasMoreItemsCore;
            }
        }
        public bool HasMoreItemsCore { get; set; } = true;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return AsyncInfo.Run((c) => LoadMoreItemsAsyncCore(c, count));
        }

        protected async virtual Task<LoadMoreItemsResult> LoadMoreItemsAsyncCore(CancellationToken cancell, uint count)
        {
            //开始加载
            LoadMoreBegain?.Invoke(null, null);

            var uloveImages = await OnlineImageManager.GetUloveImages(this.Count, (int)count);

            if (uloveImages == null || uloveImages.Count() <= 0)
                HasMoreItemsCore = false;
            else
            {
                HasMoreItemsCore = true;
                await OnlineImageManager.GetImageEntities(this, uloveImages);
            }
               

            //结束加载
            LoadMoreEnd?.Invoke(null, null);
            LoadMoreItemsResult re = new LoadMoreItemsResult()  { Count = uloveImages == null ? 0 : (uint)uloveImages.Count() };
            return re;
        }

        public event EventHandler LoadMoreBegain;
        public event EventHandler LoadMoreEnd;
    }
}