
using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile
    {
        public void StudentTakenExamProfile()
        {
            CreateMap<StudentExam, StudentTakenExamsResultModel>()
                .ForMember(dest => dest.ExamName, opt => opt.MapFrom(src => src.Exam.ExamName))
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Exam.Subject.Name))
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade));
        }
    }
}
