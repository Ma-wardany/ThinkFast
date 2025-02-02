
namespace OnlineExam.Domain.Entities
{
    public class Answer
    {
        public int AnswerId          { get; set; }
        public string StudentId      { get; set; }
        public int ExamId            { get; set; }
        public int QuestionId        { get; set; }
        public char SelectedAnswer   { get; set; }
        public DateTime Timestamp    { get; set; } = DateTime.Now;

        // Navigation Properties
        public Student Student { get; set; }
        public Exam Exam { get; set; }
        public Question Question { get; set; }
    }
}
