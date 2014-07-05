﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using FaceGame.Annotations;
using FaceGame.ApiInteraction;

namespace FaceGame.ModelViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly IApiClient _apiClient;
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

        private int _score;
        public int Score
        {
            get { return _score; }
            set { _score = value; NotifyPropertyChanged(); }
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
            _apiClient = new ApiClient(settings);

            Buttons = new ObservableCollection<ButtonViewModel>();
            IsQuestionLoaded = false;
            Score = 0;
        }

        public async void LoadNextQuestion()
        {
            IsLoading = true;
            Buttons.Clear();

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
        }

        public void ThrowNetworkException()
        {
            throw new HttpRequestException("Failed to fetch image"); 
        }

        public async Task<bool> Select(string tag)
        {
            var vote = await _apiClient.Vote(tag);
            Score = vote.Score;
            return true;
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