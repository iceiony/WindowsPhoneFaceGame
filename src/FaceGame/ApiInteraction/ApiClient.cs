using System;
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

        private readonly HttpClient _webClient;

        public ApiClient(AppSettings settings)
        {
            _rootUrl =  new Uri(settings.RootApiUrl);
            _userQuizPath = "";

            _webClient = new HttpClient();
            _webClient.DefaultRequestHeaders.Add("Accept", "application/json");
            Debug.WriteLine(_webClient.DefaultRequestHeaders.Accept);
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
    }
}