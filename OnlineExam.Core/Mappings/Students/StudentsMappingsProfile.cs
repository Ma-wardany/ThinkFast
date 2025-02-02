using AutoMapper;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile : Profile
    {
        public StudentsMappingsProfile()
        {
            StudentResultsQueryProfile();
            UpdateStudentCommandProfile();
            GetStudentProfile();
            StudentTakenExamProfile();
            PendingAbsentExamResultProfile();
        }
    }
}
