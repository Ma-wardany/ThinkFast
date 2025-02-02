
namespace OnlineExam.Domain.Entities
{
    public class Question
    {
        public int Id               { get; set; }
        public string Content       { get; set; }
        public string OptionA       { get; set; }
        public string OptionB       { get; set; }
        public string OptionC       { get; set; }
        public string OptionD       { get; set; }
        public char CorrectAnswer   { get; set; }
        public int ExamId           { get; set; }
        public Exam Exam            { get; set; }

        // Navigation property for student answers
        public List<Answer> Answers { get; set; }
    }

}