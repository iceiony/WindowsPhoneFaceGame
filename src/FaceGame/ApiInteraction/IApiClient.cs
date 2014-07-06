using System.Threading.Tasks;

namespace FaceGame.ApiInteraction
{
    public interface IApiClient
    {
        Task<QuizQuestion> GetQuizOptionAync();
        Task<Vote> Vote(string votePath);
        Task<LoginResult> Register(LoginInformation registerInformation);
        Task<LoginResult> LogIn(LoginInformation registerInformation);
    }
}