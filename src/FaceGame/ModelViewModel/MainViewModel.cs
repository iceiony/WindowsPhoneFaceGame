﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Windows.Foundation.Metadata;
using FaceGame.Annotations;
using FaceGame.ApiInteraction;

namespace FaceGame.ModelViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IApiClient _apiClient;
        private AppSettings _appSettings;

        public bool IsQuestionLoaded { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; NotifyPropertyChanged(); }
        }

        private string _loadProgress;
        public string LoadProgress
        {
            get { return _loadProgress; }
            set { _loadProgress = value; NotifyPropertyChanged(); }
        }

        private string _score;
        public string Score
        {
            get { return _score; }
            set { _score = value; NotifyPropertyChanged(); }
        } 
        
        private string _voteScore;
        public string VoteScore
        {
            get { return _voteScore; }
            set { _voteScore = value; NotifyPropertyChanged(); }
        }

        private BitmapImage _currentImage;
        public BitmapImage CurrentImage
        {
            get { return _currentImage; }
            set { _currentImage = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<ButtonViewModel> Buttons { get; private set; }
       

        public MainViewModel(AppSettings settings)
        {
            _appSettings = settings;
            _apiClient = new ApiClient(settings);

            Buttons = new ObservableCollection<ButtonViewModel>();
            IsQuestionLoaded = false;
            Score = "0";
        }

        public async Task<QuizQuestion> LoadNextQuestion()
        {
            Buttons.Clear();
            IsLoading = true;

            var quizQuestion = await _apiClient.GetQuizOptionAync();
            CurrentImage = new BitmapImage(new Uri(quizQuestion.ImageSrc));

            CurrentImage.ImageFailed += (s, e) => ThrowNetworkException();
            CurrentImage.DownloadProgress += (s, p) => LoadProgress = p.Progress + "%";
            
            CurrentImage.ImageOpened += (s, e) =>
            {
                foreach (var option in quizQuestion.Links)
                {
                    Buttons.Add(new ButtonViewModel()
                    {
                        Text = option.Text,
                        Tag = option.Href
                    });
                }

                IsLoading = false;
                IsQuestionLoaded = true;
            };

            return quizQuestion;
        }

        public void ThrowNetworkException()
        {
            throw new HttpRequestException("Failed to fetch image"); 
        }

        public async Task<Vote> Select(string tag)
        {
            Buttons.Clear();

            var response = await _apiClient.Vote(tag);
            UpdateScores(response.Score, response.VoteScore);

            return response;
        }

        public void UpdateScores(int score,int voteScore)
        {
            Score = score.ToString();
            VoteScore = voteScore > 0 ? "+" + voteScore : voteScore.ToString();
        }

        public async Task<LoginResult> Authenticate(LoginInformation loginInformation, LogInType type)
        {
            IsLoading = true;

            LoginResult result;
            if (type == LogInType.LogIn)
                result = await _apiClient.LogIn(loginInformation);
            else
                result = await _apiClient.Register(loginInformation);

            if (result.IsSuccess)
            {
                _appSettings.LoginEmail = loginInformation.Email;
                _appSettings.LoginPassword = loginInformation.Password;
                UpdateScores(result.Score + result.VoteScore, result.VoteScore);
                LoadNextQuestion();
            }

            IsLoading = false;
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
