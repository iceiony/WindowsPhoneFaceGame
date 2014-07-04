using System;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace FaceGame.ApiInteraction
{

    public class ApiClient : IApiClient
    {
        private readonly string _rootUrl;
        private readonly WebClient _webClient;

        public ApiClient(AppSettings settings)
        {
            _rootUrl =  settings.RootApiUrl;

            _webClient = new WebClient();
            _webClient.Headers["Accept"] = "application/json";
        }


        public async Task<QuizQeustion> GetQuizOptionAync()
        {
            var response = await _webClient.DownloadStringTaskAsync(new Uri(_rootUrl));
            var quizQuestion = JsonConvert.DeserializeObject<QuizQeustion>(response);

            quizQuestion.ImageSrc = _rootUrl + quizQuestion.ImageSrc; 

            return quizQuestion;
        }
    }
}