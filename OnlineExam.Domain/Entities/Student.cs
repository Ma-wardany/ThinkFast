using OnlineExam.Domain.Entities.Identity;


namespace OnlineExam.Domain.Entities
{
    public class Student
    {
        public string Id                               { get; set; }
        public string FullName                         { get; set; }
        public string Gender                           { get; set; }
        public int    SchoolYear                       { get; set; }
        public virtual List<StudentExam>? StudentExams { get; set; }
        public virtual List<Answer>? Answers           { get; set; }
        public virtual List<Enrollment>? Enrollments   { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}