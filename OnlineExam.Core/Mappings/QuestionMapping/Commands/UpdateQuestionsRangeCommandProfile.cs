

using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.QuestionMapping
{
    public partial class QuestionProfile
    {
        public void UpdateQuestionsRangeCommandProfile()
        {

            CreateMap<UpdateQuestionRangeCommand, List<Question>>()
            .ConvertUsing((src, dest, context) =>
                context.Mapper.Map<List<Question>>(src.QuestionList));


            CreateMap<QuestionsUpdateDto, Question>()
                .ForMember(dest => dest.Id,            opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Content,       opt => opt.MapFrom(src => src.Question!.Content))
                .ForMember(dest => dest.OptionA,       opt => opt.MapFrom(src => src.Question!.OptionA))
                .ForMember(dest => dest.OptionB,       opt => opt.MapFrom(src => src.Question!.OptionB))
                .ForMember(dest => dest.OptionC,       opt => opt.MapFrom(src => src.Question!.OptionC))
                .ForMember(dest => dest.OptionD,       opt => opt.MapFrom(src => src.Question!.OptionD))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.Question!.CorrectAnswer));
        }
    }
}
