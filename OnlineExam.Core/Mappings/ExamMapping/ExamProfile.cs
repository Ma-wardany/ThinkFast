using AutoMapper;
using OnlineExam.Core.Features.Exams.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.ExamMapping
{
    public partial class ExamProfile : Profile
    {
        public ExamProfile()
        {
            AddExamCommandProfile();
            UpdateExamCommandProfile();

            // shared between queries and command of exam
            CreateMap<Exam, ExamResultModel>()
                .ForMember(dest => dest.ExamId,         opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.FirstName + " " + src.Instructor.LastName))
                .ForMember(dest => dest.SubjectName,    opt => opt.MapFrom(src => src.Subject.Name))
                .ForMember(dest => dest.QuestionsCount, opt => opt.MapFrom(src => src.Questions.Count()))
                .ReverseMap();
        }
    }
}
