using OnlineExam.Core.Features.Instructors.DTOs;

namespace OnlineExam.Core.Features.Instructors.Results
{
    public class GetInstructorBySubjectResultModel
    {
        public string InstructorId { get; set; }
        public string InstructorName { get; set; }
        public InstructorSubjectDto Subject { get; set; }
    }
}
