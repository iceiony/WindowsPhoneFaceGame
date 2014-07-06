using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.ApplicationModel.Store;
using FaceGame.ApiInteraction;
using FaceGame.ModelViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;

namespace FaceGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel _mainViewModel;

        public MainPage()
        {
            InitializeComponent();

            var currentApp = (App)Application.Current;
            _mainViewModel = currentApp.MainViewModel;
            DataContext = _mainViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            if (!_mainViewModel.IsQuestionLoaded)
            {
                _mainViewModel.LoadNextQuestion();
                _mainViewModel.PropertyChanged += (s, e) =>
                {
                    switch (e.PropertyName)
                    {
                        case "Score":
                            AnimateScoreChange.Begin();
                            break;
                        case "VoteScore":
                            AnimateScoreVoteChange.Begin();
                            break;
                    }
                };
            }


        }

        private async void Button_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonTag = ((Button)sender).Tag.ToString();

            await _mainViewModel.Select(buttonTag);
            _mainViewModel.LoadNextQuestion();
        }
    }
}