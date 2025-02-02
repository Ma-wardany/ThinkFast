
namespace OnlineExam.Domain.Entities
{
    public class Exam
    {
        public int Id                                 { get; set; }
        public string ExamName                        { get; set; }
        public DateTime ExamDate                      { get; set; }
        public int SchoolYear                         { get; set; }
        public string InstructorId                    { get; set; }
        public string ExamCode                        { get; set; }
        public Instructor Instructor                  { get; set; }
        public int SubjectId                          { get; set; }
        public Subject Subject                        { get; set; }
        public string Status                          { get; set; } = "pending";
        public virtual List<Question> Questions       { get; set; }
        public virtual List<StudentExam> StudentExams { get; set; }
        public virtual List<Answer> Answers           { get; set; }
    }
}
