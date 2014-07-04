using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FaceGame.ApiResponse;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace FaceGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private readonly WebClient _webClient = new WebClient();
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _webClient.Headers["Accept"] = "application/json";
        }

        protected override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            var rootUrl = ((App)Application.Current).AppSettings.RootApiUrl;

            _webClient.DownloadStringAsync(new Uri(rootUrl));

            _webClient.DownloadStringCompleted += (sender, download) =>
            {
                var quizQuestion = JsonConvert.DeserializeObject<QuizQeustion>(download.Result);
                main_image.Source = new BitmapImage(new Uri(rootUrl + quizQuestion.ImageSrc));

                foreach (var link in quizQuestion.Links)
                {
                    ButtonList.Children.Add(
                        new Button(){
                            Tag = new Uri(rootUrl + link.Href),
                            Content = link.Text
                        });
                }


                LayoutRoot.Visibility = Visibility.Visible;
                SystemTray.ProgressIndicator.IsVisible = false;
            };

        }
    }
}