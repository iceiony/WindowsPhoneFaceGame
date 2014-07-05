using System.Collections.Generic;

namespace FaceGame.ApiInteraction
{
    public class QuizQuestion
    {
        public List<QuizOption> Links { get; set; }
        public string ImageSrc { get; set; }
    }
}