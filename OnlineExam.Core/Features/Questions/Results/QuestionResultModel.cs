
using System.Text.Json.Serialization;

namespace OnlineExam.Core.Features.Questions.Results
{
    public class QuestionResultModel
    {
        public int Id         { get; set; }
        public string Content { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public char? CorrectAnswer { get; set; }
        public int ExamId { get; set; }
        public string ExamCode { get; set; }
    }
}
