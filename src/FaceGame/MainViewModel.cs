using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media.Imaging;
using FaceGame.Annotations;
using FaceGame.ApiInteraction;

namespace FaceGame
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ApiClient _apiClient;
        public bool IsQuestionLoaded { get; set; }

        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { _isLoading = value; NotifyPropertyChanged(); }
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

        public ObservableCollection<ButtonModel> Buttons { get; private set; }
       

        public MainViewModel(AppSettings settings)
        {
            _apiClient = new ApiClient(settings);

            Buttons = new ObservableCollection<ButtonModel>();
            IsQuestionLoaded = false;
            Score = 0;
        }

        public async void LoadNextQuestion()
        {
            IsLoading = true;

            var quizQuestion = await _apiClient.GetQuizOptionAync();
            CurrentImage = new BitmapImage(new Uri(quizQuestion.ImageSrc));

            Buttons.Clear();
            foreach (var option in quizQuestion.Links)
            {
                Buttons.Add(new ButtonModel()
                {
                    Text = option.Text,
                    Tag = option.Href
                });
            }

            IsLoading = false;
            IsQuestionLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class ButtonModel
    {
        public string Text { get; set; }
        public string Tag { get; set; }
    }
}