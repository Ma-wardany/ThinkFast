using OnlineExam.Domain.Entities.Identity;



namespace OnlineExam.Domain.Entities
{
    public class Instructor
    {
        public string Id                               { get; set; }
        public string FirstName                        { get; set; }
        public string LastName                         { get; set; }
        public virtual List<Subject> Subjects          { get; set; }
        public virtual List<Exam> Exams                { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}