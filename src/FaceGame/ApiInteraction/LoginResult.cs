namespace FaceGame.ApiInteraction
{
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