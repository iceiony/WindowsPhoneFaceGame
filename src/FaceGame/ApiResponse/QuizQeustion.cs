using System.Collections.Generic;

namespace FaceGame.ApiResponse
{
    public class QuizQeustion
    {
        public List<QuizOption> Links { get; set; }
        public string ImageSrc { get; set; }
    }

    public class QuizOption
    {
        public string Href { get; set; }
        public string Text { get; set; }
    }
}