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
using Windows.ApplicationModel.Store;
using FaceGame.ApiInteraction;
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
            
            var currentApp = (App) Application.Current;
            _mainViewModel = currentApp.MainViewModel;
            DataContext = _mainViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            if (!_mainViewModel.IsQuestionLoaded) 
                _mainViewModel.LoadNextQuestion();
        }
    }
}