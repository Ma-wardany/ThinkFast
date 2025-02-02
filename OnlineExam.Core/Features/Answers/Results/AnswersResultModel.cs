
namespace OnlineExam.Core.Features.Answers.Results
{
    public class AnswersResultModel
    {
        public List<AnswerDto> Answers { get; set; }
        public int Grade => CalculateGrade();

        // Method to compute grade
        private int CalculateGrade()
        {
            if (Answers == null || Answers.Count == 0)
                return 0;

            int correctAnswersCount = Answers.Count(a => a.Selected == a.Correct);
            return (int)((double)correctAnswersCount / Answers.Count * 100); // Calculate grade as percentage
        }
    }

    public class AnswerDto
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public char Selected { get; set; }
        public char Correct { get; set; }
    }
}
