using OnlineExam.Core.Features.Questions.Commands.Models;
using OnlineExam.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExam.Core.Mappings.QuestionMapping
{
    public partial class QuestionProfile
    {
        public void UpdateQuestionCommandProfile()
        {
            CreateMap<UpdateQuestionCommand, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionId))
                .ForMember(dest => dest.Answers, opt => opt.Ignore())
                .ForMember(dest => dest.Exam, opt => opt.Ignore());
        }
    }
}
