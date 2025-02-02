using OnlineExam.Core.Features.Students.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.Students
{
    public partial class StudentsMappingsProfile
    {
        public void StudentResultsQueryProfile()
        {
            CreateMap<StudentExam, StudentExamsResultModel>()
                .ForMember(dest => dest.StudentId,   opt => opt.MapFrom(src => src.StudentId))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
                .ForMember(dest => dest.ExamName,    opt => opt.MapFrom(src => src.Exam.ExamName))
                .ForMember(dest => dest.SchoolYear,  opt => opt.MapFrom(src => src.Student.SchoolYear))
                .ForMember(dest => dest.Grade,       opt => opt.MapFrom(src => src.Grade))
                .ForMember(dest => dest.Gender,      opt => opt.MapFrom(src => src.Student.Gender));
        }
    }
}
