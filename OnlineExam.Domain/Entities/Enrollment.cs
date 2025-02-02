
namespace OnlineExam.Domain.Entities
{
    public class Enrollment
    {
        public string StudentId { get; set; }
        public int SubjectId    { get; set; }
        public int SchoolYear   { get; set; }
        public Student Student  { get; set; }
        public Subject Subject  { get; set; }
    }
}