using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace FaceGame.ApiInteraction
{

    public class ApiClient : IApiClient
    {
        private readonly Uri _rootUrl;
        private string _userQuizPath;
        private AppSettings _settings;

        private HttpClient _webClient;

        public ApiClient(AppSettings settings)
        {
            _settings = settings;
            _rootUrl =  new Uri(settings.RootApiUrl);
            _userQuizPath = "";

            _webClient = new HttpClient();
            _webClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }


        public async Task<QuizQuestion> GetQuizOptionAync()
        {
            var response = await _webClient.GetStringAsync(new Uri(_rootUrl , _userQuizPath));
            var quizQuestion = JsonConvert.DeserializeObject<QuizQuestion>(response);

            quizQuestion.ImageSrc = _rootUrl + quizQuestion.ImageSrc; 

            return quizQuestion;
        }

        public async Task<Vote> Vote(string votePath)
        {
            var response = await _webClient.GetStringAsync(new Uri(_rootUrl, votePath));
            var vote = JsonConvert.DeserializeObject<Vote>(response);
            _userQuizPath = vote.QuizLink;

            return vote;
        }

        public Task<LoginResult> Register(LoginInformation registerInformation)
        {
            return PostLoginInformation(registerInformation, "/register");
        }  
        
        public Task<LoginResult> LogIn(LoginInformation loginInformation)
        {
            return PostLoginInformation(loginInformation, "/login");
        }

        public async Task<LoginResult> PostLoginInformation(LoginInformation loginInformation,string path)
        {
            var postContent = new FormUrlEncodedContent(new List<KeyValuePair<string,string>>()
            {
                new KeyValuePair<string, string>("email",loginInformation.Email),
                new KeyValuePair<string, string>("password",loginInformation.Password)
            });

            var responseContent = await _webClient.PostAsync(new Uri(_rootUrl, path), postContent);
            var response = await responseContent.Content.ReadAsStringAsync();
            var loginResult = JsonConvert.DeserializeObject<LoginResult>(response);

            if (loginResult.IsSuccess)
                _userQuizPath = loginResult.QuizLink;

            return loginResult;
        }
    }

    public class LoginResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string QuizLink { get; set; }
        public int Score { get; set; }
        public int VoteScore { get; set; }
    }
}