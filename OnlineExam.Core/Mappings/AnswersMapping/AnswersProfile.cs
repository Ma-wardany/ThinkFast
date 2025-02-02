
using AutoMapper;
using OnlineExam.Core.Features.Answers.Results;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.AnswersMapping
{
    public partial class AnswersProfile : Profile
    {
        public AnswersProfile()
        {
            SubmitAnswersCommandProfile();

            // Map Answer to AnswerDto
            CreateMap<Answer, AnswerDto>()
                .ForMember(dest => dest.QuestionId, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Question.Content))
                .ForMember(dest => dest.Selected, opt => opt.MapFrom(src => src.SelectedAnswer))
                .ForMember(dest => dest.Correct, opt => opt.MapFrom(src => src.Question.CorrectAnswer));

            // Map List<Answer> to AnswersResultModel with a custom resolver
            CreateMap<List<Answer>, AnswersResultModel>()
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src));
        }
    }

}
