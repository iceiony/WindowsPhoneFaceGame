using System.Threading.Tasks;

namespace FaceGame.ApiInteraction
{
    public interface IApiClient
    {
        Task<QuizQeustion> GetQuizOptionAync();
    }
}