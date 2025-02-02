
namespace OnlineExam.Domain.Entities
{
    public class Subject
    {
        public int Id                        { get; set; }
        public string Name                   { get; set; }
        public string Code                   { get; set; }
        public string? InstructorId          { get; set; }
        public int SchoolYear                { get; set; }
        public Instructor? Instructor        { get; set; }
        public List<Exam> Exams              { get; set; }
        public List<Enrollment>? Enrollments { get; set; }
    }
}