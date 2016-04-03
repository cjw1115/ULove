using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using ULove.Models;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Http;
// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace ULove.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SinaWeiboLoginView : Page
    {
        private readonly string key = "1189519362";
        private readonly string secret = "03e2f87874a5ae0dbd2b12ec0376c7cd";
        private readonly string redirect = "http://uloveweb.azurewebsites.net/api/back/weibo";
        private readonly string authorize = "https://api.weibo.com/OAuth2/authorize ";
        private readonly string access_token = "https://api.weibo.com/OAuth2/access_token";
        private string _code;
        private ULove.Services.WeiboUser _user;

        private Random rand = null;
        
        public SinaWeiboLoginView()
        {
            rand = new Random();
            NavigationCacheMode = NavigationCacheMode.Disabled;
            this.InitializeComponent();
            progressRing.Visibility = Visibility.Collapsed;
            
            webView.NavigationStarting += WebView_NavigationStarting;
            webView.NavigationCompleted+=(o,e)=> progressRing.Visibility = Visibility.Collapsed;
        }

        private async void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            progressRing.Visibility = Visibility.Visible;

            string a = args.Uri.ToString();
            var pattern = @"code=([\d|a-zA-Z]*)";
            if (Regex.IsMatch(args.Uri.Query, pattern))
            {
                args.Cancel = true;
                _code = Regex.Match(args.Uri.Query, pattern).Groups[1].Value;

                string uri = "api/back/weibo";

                HttpClientHandler handler = new HttpClientHandler();
                handler.UseCookies = true;
                handler.CookieContainer = new System.Net.CookieContainer();
                //handler.CookieContainer.Add(new Uri(App.rootUri), new System.Net.Cookie("name","value"));
                

                HttpClient client = new HttpClient(handler);
                
                var response = await client.GetAsync(new Uri($"{App.rootUri}{uri}?code={_code}"));
                var jsonString = await response.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<Result>(jsonString);
                
                UserEntity user = null;
                if (json != null && json.Data != null)
                {
                    user = JsonConvert.DeserializeObject<UserEntity>(json.Data.ToString());
                    DAL.LocalUserDAL.SetLocalSetting<string>("Token", user.Token);
                    await Manager.UserManager.SetUser(user);
                }
                else
                {
                    notificationBar.ShowMessage(json?.Message);
                }
                
                Back();
                
                if (user != null)
                    OnLogined?.Invoke(this, new ULove.ViewModels.UserVM.LoginEventArgs { Status = true });
                else
                    OnLogined?.Invoke(this, new ULove.ViewModels.UserVM.LoginEventArgs { Status = false });
            }
        }

        private EventHandler OnLogined;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            OnLogined = e.Parameter as EventHandler;
            _user = new Services.WeiboUser();
            string authorize = $"https://api.weibo.com/oauth2/authorize?client_id={key}&response_type=code&redirect_uri={redirect}&time={rand.Next()}";
            
            webView.Navigate(new Uri(authorize));
        }
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            
        }
        
        private void Back()
        {
            if (this.Frame.CanGoBack)
            {
                
                Frame.GoBack();
            }
        }
        public class Result
        {
            public Object Data { get; set; }
            public string Message { get; set; }
        }
    }
}
