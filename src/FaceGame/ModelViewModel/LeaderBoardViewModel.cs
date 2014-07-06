using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FaceGame.Annotations;
using FaceGame.ApiInteraction;

namespace FaceGame.ModelViewModel
{
    public class LeaderBoardViewModel : INotifyPropertyChanged
    {
        private IApiClient _appClient;

        private IList<UserScore> _users;
        public IList<UserScore> Users { 
            get { return _users; } 
            set { _users = value; NotifyPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;

        public LeaderBoardViewModel(AppSettings settigns )
        {
            _appClient = new ApiClient(settigns);
        }

        public async void UpdateLeaderBoard()
        {
            Users = await _appClient.GetLeaderboardList();
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}