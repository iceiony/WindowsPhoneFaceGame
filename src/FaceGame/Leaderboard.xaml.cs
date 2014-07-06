using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Navigation;
using FaceGame.ModelViewModel;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace FaceGame
{
    public partial class Leaderboard : PhoneApplicationPage
    {
        private LeaderBoardViewModel _leaderBoardViewModel;

        public Leaderboard()
        {
            InitializeComponent();
            var app = (App) Application.Current;
            _leaderBoardViewModel = new LeaderBoardViewModel(app.AppSettings);
            DataContext = _leaderBoardViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            _leaderBoardViewModel.UpdateLeaderBoard();
        }
    }
}