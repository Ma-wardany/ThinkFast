using OnlineExam.Core.Features.Exams.Commands.Models;
using OnlineExam.Domain.Entities;

namespace OnlineExam.Core.Mappings.ExamMapping
{
    public partial class ExamProfile
    {
        public void AddExamCommandProfile()
        {
            CreateMap<AddExamCommand, Exam>();
        }
    }
}
