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
using FaceGame.ApiInteraction;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace FaceGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private ApiClient _apiClient;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            var currentApp = (App) Application.Current;
            _apiClient = new ApiClient(currentApp.AppSettings);
        }

        protected async override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            var quizQuestion = await _apiClient.GetQuizOptionAync();

            main_image.Source = new BitmapImage(new Uri(quizQuestion.ImageSrc));
            foreach (var link in quizQuestion.Links)
            {
                ButtonList.Children.Add(
                    new Button()
                    {
                        Tag = link.Href,
                        Content = link.Text
                    });
            }

            LayoutRoot.Visibility = Visibility.Visible;
            SystemTray.ProgressIndicator.IsVisible = false;

        }
    }
}