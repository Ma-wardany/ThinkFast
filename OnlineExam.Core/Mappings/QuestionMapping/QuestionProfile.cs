using AutoMapper;
using OnlineExam.Core.Features.Questions.DTOs;
using OnlineExam.Core.Features.Questions.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.QuestionMapping
{
    public partial class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            AddQuestionCommandProfile();
            UpdateQuestionCommandProfile();
            AddQuestionsRangeCommandProfile();
            UpdateQuestionsRangeCommandProfile();

            CreateMap<QuestionDto, Question>();

            CreateMap<Question, QuestionResultModel>()
                .ForMember(dest => dest.ExamCode, opt => opt.MapFrom(src => src.Exam.ExamCode));
        }
    }
}
