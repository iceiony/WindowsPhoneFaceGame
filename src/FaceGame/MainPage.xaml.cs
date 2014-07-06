using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using FaceGame.ApiInteraction;
using FaceGame.ModelViewModel;
using Microsoft.Phone.Controls;

namespace FaceGame
{
    public partial class MainPage : PhoneApplicationPage
    {
        private MainViewModel _mainViewModel;
        private AppSettings _appSettings;

        public MainPage()
        {
            InitializeComponent();

            var currentApp = (App)Application.Current;
            _appSettings = currentApp.AppSettings;

            _mainViewModel = new MainViewModel(_appSettings);
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

            DataContext = _mainViewModel;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs navEvent)
        {
            if (!_mainViewModel.IsQuestionLoaded)
            {
                if (_appSettings.HasLoginInformation)
                {
                    var loginInformation = new LoginInformation()
                    {
                        Email = _appSettings.LoginEmail,
                        Password = _appSettings.LoginPassword
                    };

                    _mainViewModel.Authenticate(loginInformation, LogInType.LogIn);
                }
                else
                {
                    _mainViewModel.LoadNextQuestion();
                }
            }
        }

        private async void OptionButton_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonTag = ((Button)sender).Tag.ToString();

            await _mainViewModel.Select(buttonTag);
            _mainViewModel.LoadNextQuestion();
        }

        private void Login_OnClick(object sender, EventArgs e)
        {
            var customMessageBox = new CustomMessageBox()
            {
                Title = "Remember me",
                Message = "Log In or Register",
                RightButtonContent = "Log In",
                LeftButtonContent = "Register"
            };

            customMessageBox.Content = GenerateLoinForm();
            customMessageBox.Dismissed += LogInUser;

            customMessageBox.Show();
        }

        private StackPanel GenerateLoinForm()
        {
            var panel = new StackPanel();
            panel.Children.Add(new TextBlock() { Text = "Email" });
            panel.Children.Add(new TextBox() { Name = "email" });
            panel.Children.Add(new TextBlock() { Text = "Password" });
            panel.Children.Add(new PasswordBox() { Name = "password" });
            return panel;
        }

        private async void LogInUser(object sender, DismissedEventArgs e)
        {
            if (e.Result == CustomMessageBoxResult.None)
                return;
       
            var loginInformation = ExtractNameAndPassword(sender);
            var logInType = e.Result == CustomMessageBoxResult.LeftButton ? LogInType.Reister : LogInType.LogIn;
            var loginResult = await _mainViewModel.Authenticate(loginInformation,logInType);

            if (!loginResult.IsSuccess)
            {
                var failureMessage = loginResult.Message ?? loginResult.Error;
                MessageBox.Show(failureMessage);
            }
        }

        private LoginInformation ExtractNameAndPassword(object sender)
        {
            var panel = ((CustomMessageBox)sender).Content as StackPanel;
            var nameBox = panel.Children[1] as TextBox;
            var password = panel.Children[3] as PasswordBox;

            return new LoginInformation()
            {
                Email = nameBox.Text,
                Password = password.Password
            };
        }
    }
}