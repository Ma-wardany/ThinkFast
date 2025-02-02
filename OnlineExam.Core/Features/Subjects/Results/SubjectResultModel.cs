

namespace OnlineExam.Core.Features.Subjects.Results
{
    public class SubjectResultModel
    {
        public int Id                 { get; set; }
        public string Name            { get; set; }
        public string Code            { get; set; }
        public int SchoolYear         { get; set; }
        public string? InstructorId   { get; set; }
        public string? InstructorName { get; set; }
        public int EnrollmentCount    { get; set; }
    }
}
