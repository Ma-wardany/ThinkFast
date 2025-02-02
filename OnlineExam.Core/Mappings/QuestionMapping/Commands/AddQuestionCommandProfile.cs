using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.QuestionMapping
{
    public partial class QuestionProfile
    {
        public void AddQuestionCommandProfile()
        {
            CreateMap<AddQuestionCommand, Question>()
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore())
                .ReverseMap();

        }
    }
}